using System.Diagnostics;

namespace LogService.Extension
{
    public class CustomLogException : Exception
    {
        private string _message;
        private Exception? _exception;
        public CustomLogException(string CustomMessage, Exception? OriginalException = null):base()
        {           
            this._exception = OriginalException;            
            this._message = CustomMessage;
        }
        public string CustomMessage
        {
            get { return _message; }
        }
        public Exception? OriginalException
        {
            get { return _exception; }
        }
    }
    public class CustomExceptionPrefix
    {
        public static string AdoAuthenticate_Failed = "AdoAuthenticate failed";
        public static string GetAccessToken_Failed = "Get access token failed";
        public static string CertificateError_Load_Failed = "CertificateError load failed";
        public static string CertificateError_Find_Failed = "CertificateError find failed";
        public static string CodeError_Need_To_Prepare_The_Test_Data_First = "Need to prepare the test data first";
        public static string CodeError_Element_Load_Failed = "Element load failed";
        public static string CodeError_Edit_Allow_AdminMessage_Failed = "Edit the allow admin message failed";
        public static string CodeError_Create_Message_Failed = "Create a new message failed";
        public static string CodeError_Edit_Message_Failed = "Edit a message failed";
        public static string CodeError_Delete_AllMessages_Failed = "Delete all existing messages failed";
        public static string CodeError_Validation_MessageName_Length_Failed = "Validation message name length failed";
        public static string CodeError_Set_ControllingMessages_Value_Failed = "Set controlling messages value on Overview page failed";
        public static string CodeError_Delete_AllRoles_Failed = "Delete All existing Roles failed";
        public static string CodeError_Validation_AuditLogs_Details_Failed = "Validation the audit logs details info failed";
        public static string CodeError_Filter_AuditLogs_Failed = "Filter the audit logs failed";
        public static string CodeError_Select_Message_Type_Theme_Failed = "Select message type and message theme on Create message panel failed";
        public static string CodeError_Set_Message_Configuration_Settings_Failed = "Set the configuration settings on message tab failed";
        public static string CodeError_Set_Scheduled_Configuration_Settings_Failed = "Set the configuration settings on Schedule tab failed";
        public static string CodeError_Set_Configuration_Settings_Failed = "Set the configuration settings failed";
        public static string CodeError_Expand_Node_Failed = "Expand node failed";
        public static string CodeError_Create_Profile_Failed = "Create profile failed";
        public static string CodeError_Update_Profile_Failed = "Update profile failed";
        public static string CodeError_Delete_Profile_Failed = "Delete profile failed";
        public static string CodeError_Close_Profile_Failed = "Close profile failed";
        public static string CodeError_Graph_Verify_Failed = "graph verify failed";
        public static string CodeError_Verify_Failed = "Verify failed";
        public static string CodeError_ProfileID_Is_Null = "profile id is null";
        public static string CodeError_Page_Upload_Failed = "upload failed";
        public static string CodeError_Check_Group_Failed = "check group failed";
        public static string CodeError_Check_Prerequisites_Failed = "check prerequisites failed";
        public static string CodeError_Element_Load_UpLoadFailed = "Upload Failed";
        public static string CodeError_Element_Load_SwitchLoginAndVerifyFailed = "Switch login and verify failed";
        public static string CodeError_Element_Load_InvokeAPIFailed = "Invoke api failed";
        public static string CodeError_Element_Load_FilePathIncorrect = "File path is incorrect";
        public static string CodeError_Element_Load_OverTime = "Over time";
        public static string CodeError_Element_Load_UXVerifyFailed = "UX Verify Failed";
        public static string CodeError_Element_Load_ParameterNull = "The parameter value is null";
        public static string CodeError_Configuration_Setting_Failed = "Set configuration settings failed";
        public static string NetworkError_Failed = "Network issues failed";
        public static string CodeError_Time_Conversion_Failed = "Time conversion failed";
        public static string CodeError_Assignment_To_Save_Profile_Failed = "Add group to Assignment and then to Save profile failed";
        public static string CodeError_Assignment_To_JsonFormatError_Failed = "please according to current feature's json formation to set parameters.";
        public static string CodeError_SettingValueToControl_Failed = "Failed to set a value to the control";
        public static string CodeError_List_Length_Not_Match = "Compared values length does not match";
        public static string CodeError_Invalid_Control_Type = "Invalid control type";
        public static string CodeError_StatusValidationFailed = "Status validation failed";
        public static string CodeError_CheckUserLicense_Failed = "Failed to check users license";
        public static string CodeError_CheckAssignUserToGroup_Failed = "Failed to check users assign to group";
        public static string CodeError_AssignMemberToGroup_Failed = "Assign member to group failed.";
        public static string CodeError_Download_File_Failed = "Download file failed";
        public static string CodeError_Delete_File_Failed = "Delete file failed";
        public static string CodeError_CreateOrUpdateProfile_Failed = "Create or update profile failed";
        public static string CodeError_Navigate_To_Target_Tab_Failed="Navigate to target tab failed";
        public static string CodeError_Search_Profile_Failed = "Search profile failed";
        public static string CodeError_CovertType_Failed = "Convert type failed";
        public static string CodeError_ReadFile_Failed = "Failed to read the file";
        public static string CodeError_SetSettings_Failed = "Failed to set settings or click element";
        public static string CodeError_GetElementAttribute_Failed = "Failed to get element attribute";
        public static string DisplayError_EnableStatusIsFalse = "Element status is false";
    }
}
