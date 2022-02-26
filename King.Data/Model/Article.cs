using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// ����
    /// </summary>
    public class Article : ExtFullModifyModel, IActiveModel, ISortModel, IModifyModel
    {
        [Key]
        public int ArticleId { get; set; }       
        
        /// <summary>
        /// ����
        /// </summary>
        [Required]
        [StringLength(ModelUnits.Len_250)]
        public string Title { get; set; }
        /// <summary>
        /// �ؼ���
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Keyword { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Description { get; set; }
        /// <summary>
        /// ����
        /// </summary>      
        public string Detail { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string Author { get; set; }
        /// <summary>
        /// ��Դ
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string Source { get; set; }

        [StringLength(ModelUnits.Len_250)]
        public string SourceUrl { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string LinkUrl { get; set; }
        /// <summary>
        /// ͼƬ
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string ImageUrl { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int TagsType { get; set; }
        /// <summary>
        /// ��ǩ
        /// </summary>
        public int TagsId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int Sort { get; set; } = 0;
        /// <summary>
        /// �����
        /// </summary>
        public int ViewCount { get; set; } = 0;
        /// <summary>
        /// �Ƽ�λ��
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string RecommendPosition { get; set; }
        /// <summary>
        /// �Ƿ񷢱�
        /// </summary>
        public bool IsActive { get; set; }

        public bool IsDelete { get; set; } = false;


    }
}
