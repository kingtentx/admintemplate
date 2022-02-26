using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// ����
    /// </summary> 

    public class Navigation : ExtFullModifyModel, IActiveModel, ISortModel, IModifyModel
    {
        /// <summary>
        /// ����ID
        /// </summary>
        [Key]
        public int NavigationId { get; set; }
        /// <summary>
        /// ��ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string NavigationName { get; set; }
        /// <summary>
        /// �ض�������
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string RewriteName { get; set; }       
        /// <summary>
        /// ����
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Description { get; set; }
        /// <summary>
        /// ָ����ת����
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string LinkUrl { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// �Ƿ���ҳ
        /// </summary>
        public bool IsHomePage { get; set; }
        /// <summary>
        /// �Ƿ���ʾ
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// �Ƿ񼤻�
        /// </summary>
        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

    }
}
