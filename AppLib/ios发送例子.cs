//using PushSharp;
//using PushSharp.Apple;
//using PushSharp.Core;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using System.Web;

//namespace ry_cardcarddai.AppService.siteMessageSender
//{
//    public class IOS : RYWebInterface.IRun
//    {

//        static void testsend()
//        {
//            try
//            {
//                var push = new PushBroker();
//                var appleCert = File.ReadAllBytes("d:\\zsaps.p12");
//                push.RegisterAppleService(new ApplePushChannelSettings(true, appleCert, "123456")); //.p12密码

//                //针对指定device id发送一条信息
//                push.QueueNotification(new AppleNotification()
//                                           .ForDeviceToken("0e82d7a73d9a22396ac9ec95d675f3143c2cbe69958909d2b18dc3804ba70d8d")//device id
//                                           .WithAlert("Hello2 World!")//信息内容
//                                           .WithBadge(1)
//                                           .WithSound("default"));
//                //关闭信息服务
//                push.StopAllServices();
//            }
//            catch
//            {
//            }
//        }

//        public void Run()
//        {
//            if (sending)
//                return;

//            sending = true;
//            new Thread(ready2Send).Start();
//        }

//        bool sending = false;
//        private void ready2Send()
//        {

//            try
//            {
//                //Create our push services broker
//                var push = new PushBroker();

//                //Wire up the events for all the services that the broker registers
//                push.OnNotificationSent += NotificationSent;
//                push.OnChannelException += ChannelException;
//                push.OnServiceException += ServiceException;
//                push.OnNotificationFailed += NotificationFailed;
//                push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
//                push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
//                push.OnChannelCreated += ChannelCreated;
//                push.OnChannelDestroyed += ChannelDestroyed;

//#if DEBUG
//                string filepath = System.Web.HttpRuntime.AppDomainAppPath + "/AppService/siteMessageSender/apns.p12";
//                if (File.Exists(filepath) == false)
//                    return;
//                var appleCert = File.ReadAllBytes(filepath);
//                push.RegisterAppleService(new ApplePushChannelSettings(false, appleCert, "123456")); //.p12密码
//#else
//                string filepath = System.Web.HttpRuntime.AppDomainAppPath + "/AppService/siteMessageSender/aps.p12";
//                var appleCert = File.ReadAllBytes(filepath);
//                push.RegisterAppleService(new ApplePushChannelSettings(true, appleCert, "123456")); //.p12密码
//#endif
//                List<int> sendBadgeUserIds = new List<int>();
//                using (BankDB db = new BankDB())
//                {
//                    while (RYWebInterface.BasePage.State == RYWebInterface.BasePage.WebSiteState.Running)
//                    {
//                        bool canBreak = true;
//                        int lastUserId = 0;
//                        while (true)
//                        {
//                            var users = (from m in db.tb_userInfo
//                                         where m.cDeviceToKen != null && m.id > lastUserId
//                                         orderby m.id
//                                         select new cardcarddai.Table.tb_userInfo
//                                         {
//                                             id = m.id,
//                                             dAppOnlineTime = m.dAppOnlineTime,
//                                             cDeviceToKen = m.cDeviceToKen,
//                                         }).Take(100).ToArray();
//                            if (users.Length == 0)
//                                break;
//                            lastUserId = users.LastOrDefault().id.Value;
//                            foreach (var user in users)
//                            {
//                                user.dAppOnlineTime = DateTime.Now;
//                                db.Update(user);

//                                var siteMsg = (from m in db.tb_SiteMessage
//                                               where m.IsRead == false && m.IsCheckedByApp == false && m.iUserID == user.id
//                                               orderby m.dDate
//                                               select m).FirstOrDefault();
//                                if (siteMsg != null)
//                                {
//                                    siteMsg.IsRead = true;
//                                    siteMsg.IsCheckedByApp = true;
//                                    db.Update(siteMsg);

//                                    canBreak = false;

//                                    var notification = new AppleNotification()
//                                                   .ForDeviceToken(user.cDeviceToKen)
//                                                   .WithAlert(siteMsg.cContent)
//                                                   .WithTag(siteMsg.id.GetValueOrDefault())
//                                                   .WithSound("default");
//                                    if (sendBadgeUserIds.Contains(user.id.Value) == false)
//                                    {
//                                        sendBadgeUserIds.Add(user.id.Value);
//                                        notification = notification.WithBadge(db.tb_SiteMessage.Count(m => m.IsRead == false && m.IsCheckedByApp == false && m.iUserID == user.id) + 1);
//                                    }
//                                    push.QueueNotification(notification);
//                                }
//                            }
//                        }
//                        if (canBreak)
//                            break;
//                    }
//                    push.StopAllServices();
//                }
//            }
//            catch (Exception ex)
//            {
//                using (CLog log = new CLog("APNS error "))
//                {
//                    log.Log(ex.ToString());
//                }
//            }
//            finally
//            {
//                sending = false;
//            }
//        }


//        static void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
//        {
//            //Currently this event will only ever happen for Android GCM
//            // Console.WriteLine("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
//        }

//        static void NotificationSent(object sender, INotification notification)
//        {

//            //Console.WriteLine("Sent: " + sender + " -> " + notification);
//        }

//        static void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
//        {
//            try
//            {
//                PushSharp.Apple.NotificationFailureException ex = notificationFailureException as PushSharp.Apple.NotificationFailureException;
//                if (ex.ErrorStatusCode == 8)
//                {
//                    //Invalid device token
//                    int msgid = (int)notification.Tag;
//                    using (BankDB db = new BankDB())
//                    {
//                        var item = db.tb_SiteMessage.FirstOrDefault(m => m.id == msgid);
//                        item.IsRead = false;
//                        item.IsCheckedByApp = false;
//                        db.Update(item);

//                        var user = db.tb_userInfo.FirstOrDefault(m => m.id == item.iUserID);
//                        user.cDeviceToKen = null;
//                        db.Update(user);
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                using (CLog log = new CLog("NotificationSent error "))
//                {
//                    log.Log(ex.ToString());
//                }
//            }


//            // Console.WriteLine("Failure: " + sender + " -> " + notificationFailureException.Message + " ErrorStatusDescription:" + ex.ErrorStatusDescription + " -> " + notification);
//        }

//        static void ChannelException(object sender, IPushChannel channel, Exception exception)
//        {
//            //Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
//        }

//        static void ServiceException(object sender, Exception exception)
//        {
//            //Console.WriteLine("Service Exception: " + sender + " -> " + exception);
//        }

//        static void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
//        {
//            //Console.WriteLine("Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);
//        }

//        static void ChannelDestroyed(object sender)
//        {
//            // Console.WriteLine("Channel Destroyed for: " + sender);
//        }

//        static void ChannelCreated(object sender, IPushChannel pushChannel)
//        {

//        }
//    }
//}