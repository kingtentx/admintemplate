using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using King.Data.ExtModel;

namespace King.Data
{
    /// <summary>
    /// ����Ա
    /// </summary>
    public class Admin : ExtUpdateModel, IActiveModel
    {
        /// <summary>
        /// ����ԱID
        /// </summary>
        [Key]
        public int AdminId { get; set; }
        /// <summary>
        /// ����Ա����
        /// </summary>
        [Required]
        [StringLength(ModelUnits.Len_50)]
        public string UserName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [Required]
        [StringLength(ModelUnits.Len_50)]
        public string Password { get; set; }
        /// <summary>
        /// �ֻ�
        /// </summary>      
        [StringLength(ModelUnits.Len_50)]
        public string Telphone { get; set; }
        /// <summary>
        /// ��ʵ����
        /// </summary>       
        [StringLength(ModelUnits.Len_50)]
        public string RealName { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>       
        [StringLength(ModelUnits.Len_500)]
        public string Remark { get; set; }
        /// <summary>
        /// ��ɫ
        /// </summary>       
        [StringLength(ModelUnits.Len_100)]
        public string Roles { get; set; }
        /// <summary>
        /// �Ƿ񳬼�����Ա
        /// </summary>
        public bool IsAdmin { get; set; } = false;
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsActive { get; set; }

    }
}
