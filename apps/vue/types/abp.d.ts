declare interface LocalizableStringInfo {
  resourceName: string;
  name: string;
}

declare type ExtraPropertyDictionary = { [key: string]: any };

declare interface IHasConcurrencyStamp {
  concurrencyStamp?: string;
}

declare interface IHasExtraProperties {
  extraProperties: ExtraPropertyDictionary;
}

declare interface ISelectionStringValueItem {
  value: string;
  displayText: LocalizableStringInfo;
}

declare interface ISelectionStringValueItemSource {
  items: ISelectionStringValueItem[];
}

declare interface Validator {
  name: string;
  properties: { [key: string]: string };
}

declare interface ValueType {
  name: string;
  validator: Validator;
  properties: { [key: string]: string };
}

declare interface SelectionStringValueType extends ValueType {
  itemSource: ISelectionStringValueItemSource;
}

declare interface LanguageInfo {
  cultureName: string;
  uiCultureName: string;
  displayName: string;
  twoLetterISOLanguageName: string;
  flagIcon?: string;
}

declare interface DateTimeFormat {
  calendarAlgorithmType: string;
  dateTimeFormatLong: string;
  shortDatePattern: string;
  fullDateTimePattern: string;
  dateSeparator: string;
  shortTimePattern: string;
  longTimePattern: string;
}

declare interface CurrentCulture {
  displayName: string;
  englishName: string;
  threeLetterIsoLanguageName: string;
  twoLetterIsoLanguageName: string;
  isRightToLeft: boolean;
  cultureName: string;
  name: string;
  nativeName: string;
  dateTimeFormat: DateTimeFormat;
}

declare interface WindowsTimeZone {
  timeZoneId: string;
}

declare interface IanaTimeZone {
  timeZoneName: string;
}

declare interface TimeZone {
  iana: IanaTimeZone;
  windows: WindowsTimeZone;
}

declare interface Clock {
  kind: string;
}

declare interface MultiTenancyInfo {
  isEnabled: boolean;
}

declare interface CurrentTenant {
  id?: string;
  name?: string;
  isAvailable: boolean;
}

declare interface CurrentUser {
  isAuthenticated: boolean;
  id?: string;
  tenantId?: string;
  impersonatorUserId?: string;
  impersonatorTenantId?: string;
  impersonatorUserName?: string;
  impersonatorTenantName?: string;
  userName: string;
  name?: string;
  surName?: string;
  email: string;
  emailVerified: boolean;
  phoneNumber?: string;
  phoneNumberVerified: boolean;
  roles: string[];
}
