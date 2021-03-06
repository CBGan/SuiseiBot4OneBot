using YukariToolBox.FormatLog;

namespace AntiRain.IO.Config.ConfigModule
{
    internal class GlobalConfig
    {
        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel LogLevel { set; get; }

        /// <summary>
        /// 监听地址
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public uint Port { get; set; }

        /// <summary>
        /// 鉴权Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// API请求路径
        /// </summary>
        public string ApiPath { get; set; }

        /// <summary>
        /// Event请求路径
        /// </summary>
        public string EventPath { get; set; }

        /// <summary>
        /// Universal请求路径
        /// </summary>
        public string UniversalPath { get; set; }

        /// <summary>
        /// <para>心跳包超时设置(秒)</para>
        /// <para>此值请不要小于或等于客户端心跳包的发送间隔</para>
        /// </summary>
        public uint HeartBeatTimeOut { get; set; }

        /// <summary>
        /// <para>客户端API调用超时设置(毫秒)</para>
        /// <para>默认为1000无需修改</para>
        /// </summary>
        public uint OnebotApiTimeOut { get; set; }

        /// <summary>
        /// 资源数据库配置
        /// </summary>
        public ResourceConfig ResourceConfig { set; get; }

        /// <summary>
        /// <para>API管理接口监听地址</para>
        /// </summary>
        public string AntiRainAPILocation { get; set; }

        /// <summary>
        /// <para>API管理接口监听端口</para>
        /// </summary>
        public int AntiRainAPIPort { get; set; }
    }
}