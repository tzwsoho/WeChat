using System;
using System.Runtime.Serialization;

namespace WeChat
{
    [DataContract()]
    public class WXBaseResponse
    {
        [DataMember(Name = "Ret")]
        internal int m_ret;
        [DataMember(Name = "ErrMsg")]
        internal string m_err_msg;
    }

    [DataContract()]
    public class WXBaseRequest
    {
        [DataMember(Name = "DeviceID")]
        internal string m_device_id;
        [DataMember(Name = "Sid")]
        internal string m_wxsid;
        [DataMember(Name = "Skey")]
        internal string m_skey;
        [DataMember(Name = "Uin")]
        internal long m_uin;
    }

    /*
        "Uin": 0,
        "UserName": 用户名称，一个"@"为好友，两个"@"为群组
        "NickName": 昵称(可以带表情之类，如：<span class=\"emoji emoji270a\"></span>Wade)
        "HeadImgUrl":头像图片链接地址
        "ContactFlag": 1-好友， 2-群组， 3-公众号
        "MemberCount": 成员数量，只有在群组信息中才有效,
        "MemberList": 成员列表,
        "RemarkName": 备注名称
        "HideInputBarFlag": 0,
        "Sex": 性别，0-未设置（公众号、保密），1-男，2-女
        "Signature": 公众号的功能介绍 or 好友的个性签名
        "VerifyFlag": 0,
        "OwnerUin": 0,
        "PYInitial": 用户名拼音缩写
        "PYQuanPin": 用户名拼音全拼
        "RemarkPYInitial":备注拼音缩写
        "RemarkPYQuanPin": 备注拼音全拼
        "StarFriend": 是否为星标朋友  0-否  1-是
        "AppAccountFlag": 0,
        "Statues": 0,
        "AttrStatus": 119911,
        "Province": 省
        "City": 市
        "Alias": 
        "SnsFlag": 17,
        "UniFriend": 0,
        "DisplayName": "",
        "ChatRoomId": 0,
        "KeyWord": 
        "EncryChatRoomId": ""
    */

    [DataContract()]
    public class WXMember
    {
        [DataMember(Name = "Uin", IsRequired = false)]
        internal long m_uin;
        [DataMember(Name = "UserName")]
        internal string m_user_name;
        [DataMember(Name = "NickName")]
        internal string m_nick_name;
        [DataMember(Name = "RemarkName")]
        internal string m_remark_name;
    }

    [DataContract()]
    public class WXUser : WXMember
    {
        [DataMember(Name = "HeadImgUrl")]
        internal string m_head_img_url;
        [DataMember(Name = "ContactFlag")]
        internal int m_contact_flag;
        [DataMember(Name = "VerifyFlag")]
        internal int m_verify_flag;
        [DataMember(Name = "SnsFlag")]
        internal int m_sns_flag;
        [DataMember(Name = "Sex")]
        internal int m_sex;
        [DataMember(Name = "Signature")]
        internal string m_signature;
    }

    [DataContract()]
    public class WXContact : WXUser
    {
        [DataMember(Name = "Province", IsRequired = false)]
        internal string m_province;
        [DataMember(Name = "City", IsRequired = false)]
        internal string m_city;
    }

    [DataContract()]
    public class WXBatchContact : WXMember
    {
        [DataMember(Name = "DisplayName")]
        internal string m_display_name;
    }

    [DataContract()]
    public class WXModMember
    {
        [DataMember(Name = "UserName")]
        internal string m_user_name;
        [DataMember(Name = "NickName")]
        internal string m_nick_name;
        [DataMember(Name = "DisplayName")]
        internal string m_display_name;
    }

    [DataContract()]
    public class WXModContact : WXContact
    {
        [DataMember(Name = "ChatRoomOwner")]
        internal string m_chat_room_owner;
        [DataMember(Name = "MemberCount")]
        internal int m_member_count;
        [DataMember(Name = "MemberList")]
        internal WXModMember[] m_member_list;
    }

    [DataContract()]
    public class WXContactsRes
    {
        [DataMember(Name = "BaseResponse")]
        internal WXBaseResponse m_base_response;
        [DataMember(Name = "MemberCount")]
        internal int m_member_count;
        [DataMember(Name = "MemberList")]
        internal WXContact[] m_member_list;
        [DataMember(Name = "Seq")]
        internal int m_seq;
    }

    [DataContract()]
    public class WXBatchContactListItem
    {
        [DataMember(Name = "ChatRoomId")]
        internal string m_chat_room_id;
        [DataMember(Name = "UserName")]
        internal string m_user_name;
    }

    [DataContract()]
    public class WXBatchContacts
    {
        [DataMember(Name = "BaseRequest")]
        internal WXBaseRequest m_base_request;
        [DataMember(Name = "Count")]
        internal int m_count;
        [DataMember(Name = "List")]
        internal WXBatchContactListItem[] m_list;
    }

    [DataContract()]
    public class WXBatchContactListItemRes : WXContact
    {
        [DataMember(Name = "EncryChatRoomId")]
        internal string m_encry_chat_room_id;
        [DataMember(Name = "MemberCount")]
        internal int m_member_count;
        [DataMember(Name = "MemberList")]
        internal WXBatchContact[] m_member_list;
    }

    [DataContract()]
    public class WXBatchContactsRes
    {
        [DataMember(Name = "BaseResponse")]
        internal WXBaseResponse m_base_response;
        [DataMember(Name = "Count")]
        internal int m_count;
        [DataMember(Name = "ContactList")]
        internal WXBatchContactListItemRes[] m_contact_list;
    }

    [DataContract()]
    public class WXSyncKeyListItem
    {
        [DataMember(Name = "Key")]
        internal int m_key;
        [DataMember(Name = "Val")]
        internal int m_val;
    }

    [DataContract()]
    public class WXSyncKey
    {
        [DataMember(Name = "Count")]
        internal int m_count;
        [DataMember(Name = "List")]
        internal WXSyncKeyListItem[] m_list;
    }

    [DataContract()]
    public class WXInit
    {
        [DataMember(Name = "BaseResponse")]
        internal WXBaseResponse m_base_response;
        [DataMember(Name = "SyncKey")]
        internal WXSyncKey m_sync_key;
        [DataMember(Name = "User")]
        internal WXUser m_user;
        [DataMember(Name = "ContactList")]
        internal WXContact[] m_contact_list;
    }

    [DataContract()]
    public class WXInitReq
    {
        [DataMember(Name = "BaseRequest")]
        internal WXBaseRequest m_base_request;
    }

    [DataContract()]
    public class WXStatusNotify
    {
        [DataMember(Name = "BaseRequest")]
        internal WXBaseRequest m_base_request;
        [DataMember(Name = "Code")]
        internal int m_code;
        [DataMember(Name = "FromUserName")]
        internal string m_from_user_name;
        [DataMember(Name = "ToUserName")]
        internal string m_to_user_name;
        [DataMember(Name = "ClientMsgId")]
        internal long m_client_msg_id;
    }

    [DataContract()]
    public class WXRecommendInfo
    {
        [DataMember(Name = "UserName")]
        internal string m_user_name;
        [DataMember(Name = "NickName")]
        internal string m_nick_name;
        [DataMember(Name = "QQNum")]
        internal int m_qq_num;
        [DataMember(Name = "Province")]
        internal string m_province;
        [DataMember(Name = "City")]
        internal string m_city;
        [DataMember(Name = "Content")]
        internal string m_content;
        [DataMember(Name = "Signature")]
        internal string m_signature;
        [DataMember(Name = "Alias")]
        internal string m_alias;
        [DataMember(Name = "Scene")]
        internal int m_scene;
        [DataMember(Name = "VerifyFlag")]
        internal int m_verify_flag;
        [DataMember(Name = "AttrStatus")]
        internal int m_attr_status;
        [DataMember(Name = "Sex")]
        internal int m_sex;
        [DataMember(Name = "Ticket")]
        internal string m_ticket;
        [DataMember(Name = "OpCode")]
        internal int m_op_code;
    }

    [DataContract()]
    public class WXAppInfo
    {
        [DataMember(Name = "AppID")]
        internal string m_app_id;
        [DataMember(Name = "Type")]
        internal int m_type;
    }

    [DataContract()]
    public class WXMsg
    {
        [DataMember(Name = "MsgId")]
        internal string m_msg_id;
        [DataMember(Name = "FromUserName")]
        internal string m_from_user_name;
        [DataMember(Name = "ToUserName")]
        internal string m_to_user_name;
        [DataMember(Name = "MsgType")]
        internal int m_msg_type;
        [DataMember(Name = "Content")]
        internal string m_content;
        [DataMember(Name = "Status")]
        internal int m_status;
        [DataMember(Name = "ImgStatus")]
        internal int m_img_status;
        [DataMember(Name = "CreateTime")]
        internal int m_create_time;
        [DataMember(Name = "VoiceLength")]
        internal int m_voice_length;
        [DataMember(Name = "PlayLength")]
        internal int m_play_length;
        [DataMember(Name = "FileName")]
        internal string m_file_name;
        [DataMember(Name = "FileSize")]
        internal string m_file_size;
        [DataMember(Name = "MediaId")]
        internal string m_media_id;
        [DataMember(Name = "Url")]
        internal string m_url;
        [DataMember(Name = "AppMsgType")]
        internal int m_app_msg_type;
        [DataMember(Name = "StatusNotifyCode")]
        internal int m_status_notify_code;
        [DataMember(Name = "StatusNotifyUserName")]
        internal string m_status_notify_user_name;
        [DataMember(Name = "RecommendInfo")]
        internal WXRecommendInfo m_recommend_info;
        [DataMember(Name = "ForwardFlag")]
        internal int m_forward_flag;
        [DataMember(Name = "AppInfo")]
        internal WXAppInfo m_app_info;
        [DataMember(Name = "HasProductId")]
        internal int m_has_product_id;
        [DataMember(Name = "Ticket")]
        internal string m_ticket;
        [DataMember(Name = "ImgHeight")]
        internal int m_img_height;
        [DataMember(Name = "ImgWidth")]
        internal int m_img_width;
        [DataMember(Name = "SubMsgType")]
        internal int m_sub_msg_type;
        [DataMember(Name = "NewMsgId")]
        internal long m_new_msg_id;
        [DataMember(Name = "OriContent")]
        internal string m_ori_content;
        [DataMember(Name = "EncryFileName")]
        internal string m_encry_file_name;
    }

    [DataContract()]
    public class WXSync
    {
        [DataMember(Name = "BaseResponse")]
        internal WXBaseResponse m_base_response;
        [DataMember(Name = "SyncKey")]
        internal WXSyncKey m_sync_key;
        [DataMember(Name = "SyncCheckKey")]
        internal WXSyncKey m_sync_check_key;
        [DataMember(Name = "AddMsgCount")]
        internal int m_add_msg_count;
        [DataMember(Name = "AddMsgList")]
        internal WXMsg[] m_add_msg_list;
        [DataMember(Name = "ModContactCount")]
        internal int m_mod_contact_count;
        [DataMember(Name = "ModContactList")]
        internal WXModContact[] m_mod_contact_list;
        [DataMember(Name = "DelContactCount")]
        internal int m_del_contact_count;
        [DataMember(Name = "DelContactList")]
        internal WXModContact[] m_del_contact_list;
        [DataMember(Name = "ModChatRoomMemberCount")]
        internal int m_mod_chat_room_member_count;
        [DataMember(Name = "ModChatRoomMemberList")]
        internal WXModContact[] m_mod_chat_room_member_list;
    }

    [DataContract()]
    public class WXSyncReq
    {
        [DataMember(Name = "BaseRequest")]
        internal WXBaseRequest m_base_request;
        [DataMember(Name = "SyncKey")]
        internal WXSyncKey m_sync_key;
        [DataMember(Name = "rr")]
        internal long m_rr;
    }

    [DataContract()]
    public class WXMsgContent
    {
        [DataMember(Name = "ClientMsgId")]
        internal string m_client_msg_id;
        [DataMember(Name = "Content")]
        internal string m_content;
        [DataMember(Name = "FromUserName")]
        internal string m_from_user_name;
        [DataMember(Name = "LocalID")]
        internal string m_local_id;
        [DataMember(Name = "ToUserName")]
        internal string m_to_user_name;
        [DataMember(Name = "Type")]
        internal int m_type;
    }

    [DataContract()]
    public class WXSendMsg
    {
        [DataMember(Name = "BaseRequest")]
        internal WXBaseRequest m_base_request;
        [DataMember(Name = "Msg")]
        internal WXMsgContent m_msg;
        [DataMember(Name = "Scene")]
        internal int m_scene;
    }
}
