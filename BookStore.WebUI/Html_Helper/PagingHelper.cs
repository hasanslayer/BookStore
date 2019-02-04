using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BookStore.WebUI.Models;

namespace BookStore.WebUI.Html_Helper
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a"); // to dealing with HTML tags (hr,a,img,...)
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pageInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}