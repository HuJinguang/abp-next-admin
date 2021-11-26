﻿using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper.ExceptionHandling
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(AbpExceptionPageFilter))]
    public class AbpExceptionPageWrapResultFilter: AbpExceptionPageFilter, ITransientDependency
    {
        protected override bool ShouldHandleException(PageHandlerExecutingContext context)
        {
            if (!context.ActionDescriptor.CanWarpRsult())
            {
                return false;
            }
            return base.ShouldHandleException(context);
        }

        protected override async Task HandleAndWrapException(PageHandlerExecutedContext context)
        {
            var wrapResultChecker = context.GetRequiredService<IWrapResultChecker>();
            if (!wrapResultChecker.WrapOnException(context))
            {
                await base.HandleAndWrapException(context);
                return;
            }

            var wrapResultOptions = context.GetRequiredService<IOptions<AbpAspNetCoreMvcWrapperOptions>>().Value;
            var exceptionHandlingOptions = context.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;
            var exceptionToErrorInfoConverter = context.GetRequiredService<IExceptionToErrorInfoConverter>();
            var remoteServiceErrorInfo = exceptionToErrorInfoConverter.Convert(context.Exception, exceptionHandlingOptions.SendExceptionsDetailsToClients);

            var logLevel = context.Exception.GetLogLevel();

            var remoteServiceErrorInfoBuilder = new StringBuilder();
            remoteServiceErrorInfoBuilder.AppendLine($"---------- {nameof(RemoteServiceErrorInfo)} ----------");
            remoteServiceErrorInfoBuilder.AppendLine(context.GetRequiredService<IJsonSerializer>().Serialize(remoteServiceErrorInfo, indented: true));

            context.HttpContext.Response.Headers.Add(AbpHttpWrapConsts.AbpWrapResult, "true");

            var logger = context.GetService<ILogger<AbpExceptionPageWrapResultFilter>>(NullLogger<AbpExceptionPageWrapResultFilter>.Instance);
            logger.LogWithLevel(logLevel, remoteServiceErrorInfoBuilder.ToString());

            logger.LogException(context.Exception, logLevel);

            await context.GetRequiredService<IExceptionNotifier>().NotifyAsync(new ExceptionNotificationContext(context.Exception));

            // Warp Error Response
            string errorCode = remoteServiceErrorInfo.Code;
            if (context.Exception is IHasErrorCode exceptionWithErrorCode)
            {
                if (!exceptionWithErrorCode.Code.IsNullOrWhiteSpace() &&
                    exceptionWithErrorCode.Code.Contains(":"))
                {
                    errorCode = exceptionWithErrorCode.Code.Split(':')[1];
                }
                else
                {
                    errorCode = exceptionWithErrorCode.Code;
                }
            }
            context.Result = new ObjectResult(new WrapResult(errorCode, remoteServiceErrorInfo.Message, remoteServiceErrorInfo.Details));

            context.Exception = null; //Handled!
        }
    }
}
