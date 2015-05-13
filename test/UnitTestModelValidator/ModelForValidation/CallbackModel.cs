using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infra.ModelValidator;

namespace UnitTestModelValidator.ModelForValidation
{
    /// <summary>
    /// 檔案搬移狀態的更新資訊 Model
    /// </summary>
    public class CallbackModel
    {
        /// <summary>
        /// 搬檔更新項目資訊
        /// </summary>
        [NestedValidation]
        public List<CallbackModelItem> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackModel"/> class.
        /// </summary>
        public CallbackModel()
        {
            this.Items = new List<CallbackModelItem>();
        }
    }
}