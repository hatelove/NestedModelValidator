using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnitTestModelValidator.ModelForValidation
{
    /// <summary>
    /// 單一檔案搬移狀態的更新資訊 Model
    /// </summary>
    public class CallbackModelItem
    {
        /// <summary>
        /// 搬檔的來源檔路徑
        /// </summary>
        [Required(ErrorMessage = "The SourceFilePath parameter is required.")]
        public string SourceFilePath { get; set; }

        /// <summary>
        /// 搬檔的目的檔路徑
        /// </summary>
        [Required(ErrorMessage = "The DestinationFilePath parameter is required.")]
        public string DestinationFilePath { get; set; }

        /// <summary>
        /// 搬檔狀態
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// 如果搬檔失敗，記錄錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}