import 'package:core/modularity/index.dart';
import 'package:get/get.dart';
import 'package:notifications/services/index.dart';

import './pages/route.notification.dart';

class NotificationsModule extends Module {
  @override
  List<GetPage> get routes => NotificationRoute.routes;
  
  @override
  void configureServices() {
    inject<NotificationsModule>(this);
    lazyInject(() => NotificationService(), fenix: true);
    lazyInject(() => NotificationStateService(), fenix: true);
  }
}