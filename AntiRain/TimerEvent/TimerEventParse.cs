using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AntiRain.TimerEvent.Event;
using Sora.EventArgs.SoraEvent;
using Sora.EventArgs.WebsocketEvent;
using YukariToolBox.FormatLog;

namespace AntiRain.TimerEvent
{
    internal static class TimerEventParse
    {
        #region 计时器

        /// <summary>
        /// 计时器列表
        /// 一般情况下应该只有一个账号接入
        /// Key为接入的账号id
        /// </summary>
        private static readonly Dictionary<long, Timer> Timers = new Dictionary<long, Timer>();

        #endregion

        #region 计时器初始化/停止

        /// <summary>
        /// 添加新的计时器
        /// </summary>
        /// <param name="connectEventArgs">ConnectEventArgs</param>
        /// <param name="updateSpan">定时时长</param>
        internal static void TimerAdd(ConnectEventArgs connectEventArgs, uint updateSpan)
        {
            Timers.Add(connectEventArgs.LoginUid,
                       new Timer(SubscriptionEvent,                         //事件处理
                                 connectEventArgs,                          //初始化数据
                                 new TimeSpan(0),                           //即刻执行
                                 new TimeSpan(0, 0, 0, (int) updateSpan))); //设置刷新间隔
        }

        /// <summary>
        /// 删除定时器
        /// </summary>
        /// <param name="sender">消息源</param>
        /// <param name="connectionEventArgs">ConnectEventArgs</param>
        internal static ValueTask StopTimer(Guid sender, ConnectionEventArgs connectionEventArgs)
        {
            if(Timers.All(timer => timer.Key != connectionEventArgs.SelfId)) return ValueTask.CompletedTask;
            //停止计时器
            Timers[connectionEventArgs.SelfId].Dispose();
            Timers.Remove(connectionEventArgs.SelfId);
            Log.Debug("SubTimer", $"Timer stopped user[{connectionEventArgs.SelfId}]");
            return ValueTask.CompletedTask;
        }

        #endregion

        #region 更新事件方法

        /// <summary>
        /// 提醒DD动态更新的事件
        /// </summary>
        /// <param name="msgObject">CQApi</param>
        private static void SubscriptionEvent(object msgObject)
        {
            if (msgObject is ConnectEventArgs connectEventArgs)
            {
                SubscriptionUpdate.BiliUpdateCheck(connectEventArgs);
            }
        }

        #endregion
    }
}