using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AntiRain.TypeEnum.CommandType;

namespace AntiRain.Command
{
    internal static class CommandAdapter
    {
        #region 正则匹配字典

        /// <summary>
        /// 关键字匹配模式字典
        /// </summary>
        private static readonly Dictionary<KeywordCommand, List<Regex>> KeywordList =
            new Dictionary<KeywordCommand, List<Regex>>();

        /// <summary>
        /// 正则匹配模式字典
        /// </summary>
        private static readonly Dictionary<RegexCommand, List<Regex>> RegexList =
            new Dictionary<RegexCommand, List<Regex>>();

        /// <summary>
        /// 机器人指令字典
        /// </summary>
        private static readonly Dictionary<PCRGuildBattleCommand, List<Regex>> PCRGuildBattleCommandList =
            new Dictionary<PCRGuildBattleCommand, List<Regex>>();

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化关键字字典
        /// </summary>
        public static void KeywordResourseInit()
        {
            //遍历所有属性
            FieldInfo[] fieldInfos = typeof(KeywordCommand).GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                //跳过不是枚举类型的属性
                if (fieldInfo.FieldType != typeof(KeywordCommand)) continue;
                DescriptionAttribute descAttr =
                    fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).First() as DescriptionAttribute;
                //生成正则表达式列表
                List<Regex> regexes = (descAttr?.Description ?? "").Split(" ")
                                                                   .Select(keyStr => new Regex($"({keyStr})+"))
                                                                   .ToList();
                //添加到匹配列表
                KeywordList.Add((KeywordCommand) (fieldInfo.GetValue(null) ?? -1), regexes);
            }
        }

        /// <summary>
        /// 初始化正则表达式字典
        /// </summary>
        public static void RegexResourseInit()
        {
            //遍历所有属性
            FieldInfo[] fieldInfos = typeof(RegexCommand).GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                //跳过不是枚举类型的属性
                if (fieldInfo.FieldType != typeof(RegexCommand)) continue;
                DescriptionAttribute descAttr =
                    fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).First() as DescriptionAttribute;
                //生成正则表达式列表
                List<Regex> regexes = (descAttr?.Description ?? "").Split(" ")
                                                                   .Select(regexStr => new Regex(regexStr))
                                                                   .ToList();
                //添加到匹配列表
                RegexList.Add((RegexCommand) (fieldInfo.GetValue(null) ?? -1), regexes);
            }
        }

        /// <summary>
        /// 初始化机器人指令字典
        /// </summary>
        public static void PCRGuildBattlecmdResourseInit()
        {
            //遍历所有属性
            FieldInfo[] fieldInfos = typeof(PCRGuildBattleCommand).GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                //跳过不是枚举类型的属性
                if (fieldInfo.FieldType != typeof(PCRGuildBattleCommand)) continue;
                DescriptionAttribute descAttr =
                    fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).First() as DescriptionAttribute;
                //生成正则表达式列表
                List<Regex> regexes = (descAttr?.Description ?? "").Split(" ")
                                                                   .Select(cmdStr => new Regex($@"^(?:#|＃){cmdStr}.*"))
                                                                   .ToList();
                //添加到匹配列表
                PCRGuildBattleCommandList.Add((PCRGuildBattleCommand) (fieldInfo.GetValue(null) ?? -1), regexes);
            }
        }

        #endregion

        #region 获取指令类型

        /// <summary>
        /// 获取关键词模式匹配指令类型
        /// </summary>
        /// <param name="rawString">消息字符串</param>
        /// <param name="commandType">触发类型</param>
        /// <returns>匹配是否成功</returns>
        public static bool GetKeywordType(string rawString, out KeywordCommand commandType)
        {
            IEnumerable<KeywordCommand> matchResult = KeywordList
                                                      .Where(regexList =>
                                                                 regexList.Value.Any(regex => regex.IsMatch(rawString)))
                                                      .Select(regexList => regexList.Key)
                                                      .ToList();
            if (!matchResult.Any())
            {
                commandType = (KeywordCommand) (-1);
                return false;
            }
            else
            {
                commandType = matchResult.First();
                return true;
            }
        }

        /// <summary>
        /// 获取正则模式匹配指令类型
        /// </summary>
        /// <param name="rawString">消息字符串</param>
        /// <param name="commandType">触发类型</param>
        /// <returns>匹配是否成功</returns>
        public static bool GetRegexType(string rawString, out RegexCommand commandType)
        {
            IEnumerable<RegexCommand> matchResult = RegexList
                                                    .Where(regexList =>
                                                               regexList.Value.Any(regex => regex.IsMatch(rawString)))
                                                    .Select(regexList => regexList.Key)
                                                    .ToList();
            if (!matchResult.Any())
            {
                commandType = (RegexCommand) (-1);
                return false;
            }
            else
            {
                commandType = matchResult.First();
                return true;
            }
        }

        /// <summary>
        /// 获取会战管理指令类型
        /// </summary>
        /// <param name="rawString">消息字符串</param>
        /// <param name="guildBattleCommandType">指令类型</param>
        /// <returns>匹配是否成功</returns>
        public static bool GetPCRGuildBattlecmdType(string rawString, out PCRGuildBattleCommand guildBattleCommandType)
        {
            IEnumerable<PCRGuildBattleCommand> matchResult = PCRGuildBattleCommandList
                                                             .Where(regexList =>
                                                                        regexList.Value.Any(regex =>
                                                                            regex.IsMatch(rawString)))
                                                             .Select(regexList => regexList.Key)
                                                             .ToList();
            if (!matchResult.Any())
            {
                guildBattleCommandType = (PCRGuildBattleCommand) (-1);
                return false;
            }
            else
            {
                guildBattleCommandType = matchResult.First();
                return true;
            }
        }

        #endregion
    }
}