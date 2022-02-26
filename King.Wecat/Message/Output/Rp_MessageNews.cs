using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace King.Wecat.Models
{
    public class Rp_MessageNews : MessageBase
    {
        /// <summary>
        /// 图文消息个数；当用户发送文本、图片、视频、图文、地理位置这五种消息时，
        /// 开发者只能回复1条图文消息；其余场景最多可回复8条图文消息
        /// </summary>
        public int ArticleCount { get; set; } = 0;
        /// <summary>
        /// 图文消息信息，注意，如果图文数超过限制，则将只发限制内的条数
        /// </summary>
        public List<Article> Articles { get; set; }

        public override string ToXml()
        {
            XElement element = new XElement("xml");
            element.SetElementValue(nameof(ToUserName), $"<![CDATA[{ToUserName}]]>");
            element.SetElementValue(nameof(FromUserName), $"<![CDATA[{FromUserName}]]>" );
            element.SetElementValue(nameof(CreateTime), CreateTime.ToString());
            element.SetElementValue(nameof(MsgType), $"<![CDATA[{MsgType}]]>");
            element.SetElementValue(nameof(ArticleCount), ArticleCount.ToString());

            XElement article = new XElement(nameof(Articles));          
            foreach (var item in Articles)
            {
                XElement items = new XElement("item");
                items.SetElementValue(nameof(item.Title), $"<![CDATA[{item.Title}]]>");
                items.SetElementValue(nameof(item.Description), $"<![CDATA[{item.Description}]]>");
                items.SetElementValue(nameof(item.PicUrl), $"<![CDATA[{item.PicUrl}]]>");
                items.SetElementValue(nameof(item.Url), $"<![CDATA[{item.Url}]]>");
                article.Add(items);
            }         
           
            element.Add(article);

            return System.Web.HttpUtility.HtmlDecode(element.ToString());
        }

        //public override void LoadXml(string data)
        //{
        //    XElement element = XElement.Parse(data);
        //    ToUserName = element.Element(nameof(ToUserName)).Value;
        //    FromUserName = element.Element(nameof(FromUserName)).Value;
        //    CreateTime = long.Parse(element.Element(nameof(CreateTime)).Value);
        //    MsgType = element.Element(nameof(MsgType)).Value;
        //    ArticleCount = int.Parse(element.Element(nameof(ArticleCount)).Value);

        //    var articles = new List<Article>();
        //    var items = from art in element.Element(nameof(Articles)).Elements("item") select art;
        //    foreach (var a in items)
        //    {
        //        Article article = new Article()
        //        {
        //            Title = a.Element(nameof(Article.Title)).Value,
        //            Description = a.Element(nameof(Article.Description)).Value,
        //            PicUrl = a.Element(nameof(Article.PicUrl)).Value,
        //            Url = a.Element(nameof(Article.Url)).Value,
        //        };
        //        articles.Add(article);
        //    }
        //    Articles = articles;
        //}

    }
    public class Article
    {
        /// <summary>
        /// 图文消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图文消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public string Url { get; set; }
    }
}
