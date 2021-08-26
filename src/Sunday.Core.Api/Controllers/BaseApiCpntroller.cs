﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sunday.Core.Model;

namespace Sunday.Core.Api.Controllers
{
    public class BaseApiCpntroller : Controller
    {
        [NonAction]
        public MessageModel<T> Success<T>(T data, string msg = "成功")
        {
            return new MessageModel<T>()
            {
                success = true,
                msg = msg,
                response = data,
            };
        }

        [NonAction]
        public MessageModel Success(string msg = "成功")
        {
            return new MessageModel()
            {
                success = true,
                msg = msg,
                response = null,
            };
        }

        [NonAction]
        public MessageModel<string> Failed(string msg = "失败", int status = 500)
        {
            return new MessageModel<string>()
            {
                success = false,
                status = status,
                msg = msg,
                response = null,
            };
        }

        [NonAction]
        public MessageModel<T> Failed<T>(string msg = "失败", int status = 500)
        {
            return new MessageModel<T>()
            {
                success = false,
                status = status,
                msg = msg,
                response = default,
            };
        }

        [NonAction]
        public MessageModel<PageModel<T>> SuccessPage<T>(int page, int dataCount, List<T> data, int pageCount, string msg = "获取成功")
        {
            return new MessageModel<PageModel<T>>()
            {
                success = true,
                msg = msg,
                response = new PageModel<T>()
                {
                    page = page,
                    dataCount = dataCount,
                    data = data,
                    pageCount = pageCount,
                }
            };
        }

        [NonAction]
        public MessageModel<PageModel<T>> SuccessPage<T>(PageModel<T> pageModel, string msg = "获取成功")
        {
            return new MessageModel<PageModel<T>>()
            {
                success = true,
                msg = msg,
                response = new PageModel<T>()
                {
                    page = pageModel.page,
                    dataCount = pageModel.dataCount,
                    data = pageModel.data,
                    pageCount = pageModel.pageCount,
                }
            };
        }
    }
}