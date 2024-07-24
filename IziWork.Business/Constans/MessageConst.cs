using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Constans
{
    public static class MessageConst
    {
        public static readonly string TOKEN_EXPIRED = "TOKEN_EXPIRED";
        public static readonly string NOT_PERMISSION_APPROVE = "NOT_PERMISSION_APPROVE";
        public static readonly string CANNOT_FIND_CURRENT_USER = "CANNOT_FIND_CURRENT_USER";

        public static readonly string CREATED_SUCCESSFULLY = "CREATE_IS_SUCCESSFULLY";
        public static readonly string UPDATE_SUCCESSFULLY = "UPDATE_IS_SUCCESSFULLY";
        public static readonly string DELETE_SUCCESSFULLY = "DELETE_IS_SUCCESSFULLY";

        public static readonly string CANNOT_FIND_ID = "CANNOT_FIND_ID";
        public static readonly string CANNOT_FIND_PARENT_ID = "CANNOT_FIND_PARENT_ID";
        public static readonly string CANNOT_FIND_DOCUMENT_ID = "CANNOT_FIND_DOCUMENT_ID";
        public static readonly string CANNOT_FIND_TYPE_ITEM = "CANNOT_FIND_TYPE_ITEM";
        public static readonly string NOT_FOUND_PARAM = "CANNOT_FIND_ANY_PARAM";
        public static readonly string NOT_FOUND_ITEM = "ITEM_IS_NOT_EXIST";
        public static readonly string STATUS_ITEM_INVALID_FOR_THIS_ACTION = "STATUS_ITEM_INVALID_FOR_THIS_ACTION";
        public static readonly string PARAM_INVALID_FOR_THIS_ACTION = "PARAM_INVALID_FOR_THIS_ACTION";
        public static readonly string NOT_FOUND_COMPANY = "NOT_FOUND_COMPANY";

        public static readonly string NOT_FOUND_PROCESSOR = "PROCESSOR_NOT_EXIST";
        public static readonly string NOT_FOUND_DEPARTMENT = "PROCESSOR_NOT_DEPARTMENT";
        public static readonly string TASK_EXTEND_IS_EXISTS = "TASK_EXTEND_IS_EXISTS";

        public static readonly string REQUIRED_NAME = "REQUIRED_NAME";
        public static readonly string REQUIRED_YEAR = "REQUIRED_YEAR";

        public static readonly string FROM_DATE_IS_INVALID = "FROM_DATE_IS_INVALID";
        public static readonly string TO_DATE_IS_INVALID = "TO_DATE_IS_INVALID";

        public static readonly string REFERENCE_NUMBER_IS_REQUIRED = "REFERENCE_NUMBER_IS_REQUIRED";
        public static readonly string REFERENCE_NUMBER_HAS_CHANGED = "REFERENCE_NUMBER_HAS_CHANGED";
        public static class CATEGORYDETAIL {
            public static readonly string NOT_FOUND_CATEGORY = "CATEGORY_NOT_EXIST";
        }
        public static class DOCUMENT
        {
            public static readonly string NOT_FOUND_REGISTRY = "REGISTRY_NOT_EXIST";
            public static readonly string NOT_FOUND_DOCUMENT = "DOCUMENT_NOT_EXIST";
            public static readonly string NOT_FOUND_DISCUSSION = "DISCUSSION_NOT_EXIST";
            public static readonly string NOT_FOUND_SECURITY_LEVEL = "SECURITY_LEVEL_NOT_EXIST";
            public static readonly string NOT_FOUND_URGENCY_LEVEL = "URGENCY_LEVEL_NOT_EXIST";
            public static readonly string NOT_FOUND_SENDING_DEPARTMENT = "SENDING_DEPARTMENT_NOT_EXIST";
            public static readonly string NOT_FOUND_REFERENCEDOCUMENTIDS = "REFERENCEDOCUMENTIDS_NOT_FOUND";
            public static readonly string REFERENCEDOCUMENTIDS_INVALID = "REFERENCEDOCUMENTIDS_INVALID";
            public static readonly string NOT_FOUND_PARENT_DISCUSSION = "PARENT_DISCUSSION_NOT_EXIST";
        }

        public static class WORKFLOW
        {
            public static readonly string CAN_ONLY_CHOOSE_ONE_WORKFLOW_TYPE = "CAN_ONLY_CHOOSE_ONE_WORKFLOW_TYPE";
            public static readonly string CANNOT_FIND_APPROVER_ID = "CANNOT_FIND_APPROVER_ID";
            public static readonly string CANNOT_FIND_WORKFLOW = "CANNOT_FIND_WORKFLOW";
            public static readonly string CANNOT_FIND_WORKFLOW_TEMPLATE = "CANNOT_FIND_WORKFLOW_TEMPLATE";
            public static readonly string CANNOT_FIND_STEP = "CANNOT_FIND_STEP";
            public static readonly string START_WORLFOW_IS_SUCCESSFULLY = "START_WORLFOW_IS_SUCCESSFULLY";
            public static readonly string APPROVE_WORLFOW_IS_SUCCESSFULLY = "APPROVE_WORLFOW_IS_SUCCESSFULLY";
            public static readonly string REJECT_WORLFOW_IS_SUCCESSFULLY = "REJECT_WORLFOW_IS_SUCCESSFULLY";
            public static readonly string CANCEL_WORLFOW_IS_SUCCESSFULLY = "CANCEL_WORLFOW_IS_SUCCESSFULLY";
            public static readonly string REQUESTTOCHANGE_WORLFOW_IS_SUCCESSFULLY = "REQUESTTOCHANGE_WORLFOW_IS_SUCCESSFULLY";
            public static readonly string WORKFLOW_IS_COMPLETED = "WORKFLOW_IS_COMPLETED";
            public static readonly string CANNOT_FIND_ANY_RUNNING_WORKFLOW = "CANNOT_FIND_ANY_RUNNING_WORKFLOW";

            public static readonly string MUST_BE_REQUIRED_ROLE = "MUST_BE_REQUIRED_ROLE";
        }

        public static class FINANCIAL
        {
            public static readonly string ACCOUNT_NO_IS_ALREADY_EXIST = "ACCOUNT_NO_IS_ALREADY_EXIST";
            public static readonly string DEBIT_ACCOUNT_IS_REQUIRED = "DEBIT_ACCOUNT_IS_REQUIRED";
            public static readonly string DEBIT_ACCOUNT_IS_NOT_EXIST = "DEBIT_ACCOUNT_IS_NOT_EXIST";
            public static readonly string CREDIT_ACCOUNT_IS_REQUIRED = "CREDIT_ACCOUNT_IS_REQUIRED";
            public static readonly string CREDIT_ACCOUNT_IS_NOT_EXIST = "CREDIT_ACCOUNT_IS_NOT_EXIST";
            public static readonly string AMOUNT_IS_REQUIRED = "AMOUNT_IS_REQUIRED";
        }
    }
}
