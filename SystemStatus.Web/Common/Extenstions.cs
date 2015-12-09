﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SystemStatus.Domain;

namespace SystemStatus.Web.Common
{
    public static class Extensions
    {  
        public static IHtmlString ModelToJsonRaw(this HtmlHelper helper)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(helper.ViewData.Model);
            return helper.Raw(json);
        }

        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Count() > 0);
            }
            return null;
        }

        public static ModelStateDictionary MergeWith<T>(this CommandResult<T> result, ModelStateDictionary modelState)
        {
            if(!result.Success)
            {
                foreach (var ekey in result.Errors)
                {
                    foreach (var val in ekey.Value)
	                {
                        modelState.AddModelError(ekey.Key, val);
	                }
                }  
            }
            return modelState;
        }
    }
}
