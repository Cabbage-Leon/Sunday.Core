using System;
using System.Collections.Generic;
using System.Linq;
using Homsom.Friday.Exceptions;
using Homsom.Friday.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sunday.Core.Api.Filter
{
    public class GloalActionWrapFilter : ApiResultWrapAttribute
    {
        private readonly List<string> _requestIgnoreList = new List<string>()
        {
        };

        private readonly List<string> _backIgnoreList = new List<string>()
        {
        };

        /// <summary>
        /// 方法执行前
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            RecordRequest(context);
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 方法执行后
        /// </summary>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            RecordResult(context);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        public override Exception OnException(Exception exception)
        {
            if (exception is DomainException domainException)//主动throw异常
            {
                //SLogHelper.WriteWarn(domainException.Message, WriteLog(domainException.Message, domainException));
            }
            else
            {
                //SLogHelper.WriteError(exception.Message, WriteLog(exception.Message, exception));
            }
            return exception;
        }

        /// <summary>
        /// 记录请求
        /// </summary>
        private void RecordRequest(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            if (context.RouteData.Values.Values.Any(item => _requestIgnoreList.Contains(item)))
            {
                return;
            }

            var keyValuePairs = new Dictionary<string, string>()
            {
                 { "Ip地址", context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()},
                 { "请求Url", request.Host.ToString() + request.Path.ToString() + request.QueryString},
                 { "方法类型 ", request.Method},
                 { "请求头", request.Headers.ToJson()},
                 { "入参", context.ActionArguments.ToJson()},
            };
            //SLogHelper.WriteInfo("HotelSearchService接口", "== 请求进入 ==", keyValuePairs);
        }

        /// <summary>
        /// 记录请求返回结果
        /// </summary>
        private void RecordResult(ActionExecutedContext context)
        {
            if (context.RouteData.Values.Values.Any(item => _backIgnoreList.Contains(item)))
            {
                return;
            }

            var keyValuePairs = new Dictionary<string, string>()
            {
                 { "请求返回结果", context.Result.ToJson()},
            };
            //SLogHelper.WriteInfo("HotelSearchService接口", "== 请求返回 ==", keyValuePairs);
        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        public string WriteLog(string throwMsg, Exception ex)
        {
            return string.Format("\r\n【错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}",
                new object[] { throwMsg, ex.GetType().Name, ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex.StackTrace });
        }
    }
}