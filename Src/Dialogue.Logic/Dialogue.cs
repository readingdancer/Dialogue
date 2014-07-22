﻿using System.Linq;
using System.Web;
using Dialogue.Logic.Application;
using Dialogue.Logic.Constants;
using Dialogue.Logic.Models;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Dialogue.Logic
{
    public static class Dialogue
    {
        private static DialogueSettings Settings(IPublishedContent forumRootNode)
        {
            if (forumRootNode != null)
            {
                var settings = new DialogueSettings();

                // Content
                settings.ForumId = forumRootNode.Id;
                settings.ForumRootUrl = forumRootNode.Url;
                settings.ForumName = forumRootNode.GetPropertyValue<string>("forumName");
                settings.ForumDescription = forumRootNode.GetPropertyValue<string>("forumDescription");
                settings.ForumRootUrlWithDomain = string.Concat(AppHelpers.ReturnCurrentDomain(), settings.ForumRootUrl);

                // Urls
                settings.TopicUrlName = forumRootNode.GetPropertyValue<string>(AppConstants.PropTopicUrlName);
                settings.MemberUrlName = forumRootNode.GetPropertyValue<string>(AppConstants.PropMemberUrlName);
                settings.DialogueUrlName = forumRootNode.GetPropertyValue<string>(AppConstants.PropDialogueUrlName);

                var loginPage = forumRootNode.Descendant(AppConstants.DocTypeLogin) ?? forumRootNode.Ancestor(AppConstants.DocTypeLogin);
                settings.LoginUrl = loginPage != null ? loginPage.Url : "Unable to find login page";

                var registerPage = forumRootNode.Descendant(AppConstants.DocTypeRegister) ?? forumRootNode.Ancestor(AppConstants.DocTypeRegister);
                settings.RegisterUrl = registerPage != null ? registerPage.Url : "Unable to find Register page";

                var createTopic = forumRootNode.Descendant(AppConstants.DocTypeCreateTopic) ?? forumRootNode.Ancestor(AppConstants.DocTypeCreateTopic);
                settings.CreateTopicUrl = createTopic != null ? createTopic.Url : "Unable to find Create Topic page";

                var editMemberUrl = forumRootNode.Descendant(AppConstants.DocTypeEditMember) ?? forumRootNode.Ancestor(AppConstants.DocTypeEditMember);
                settings.EditMemberUrl = editMemberUrl != null ? editMemberUrl.Url : "Unable to find EditMember page";

                var searchMembersUrl = forumRootNode.Descendant(AppConstants.DocTypeSearchMembers) ?? forumRootNode.Ancestor(AppConstants.DocTypeSearchMembers);
                settings.SearchMembersUrl = searchMembersUrl != null ? searchMembersUrl.Url : "Unable to find Search members page";

                var sendPrivateMessageUrl = forumRootNode.Descendant(AppConstants.DocTypeSendPrivateMessage) ?? forumRootNode.Ancestor(AppConstants.DocTypeSendPrivateMessage);
                settings.CreatePrivateMessageUrl = sendPrivateMessageUrl != null ? sendPrivateMessageUrl.Url : "Unable to find create private message page";

                // General
                settings.CloseForum = forumRootNode.GetPropertyValue<bool>("closeForum");
                settings.AllowRssFeeds = forumRootNode.GetPropertyValue<bool>("allowRssFeeds");
                settings.SuspendRegistration = forumRootNode.GetPropertyValue<bool>("suspendRegistration");
                settings.EnableSpamReporting = forumRootNode.GetPropertyValue<bool>("enableSpamReporting");
                settings.EnableMemberReporting = forumRootNode.GetPropertyValue<bool>("enableMemberReporting");
                settings.AllowEmailSubscriptions = forumRootNode.GetPropertyValue<bool>("allowEmailSubscriptions");
                settings.ManuallyAuthoriseNewMembers = forumRootNode.GetPropertyValue<bool>("manuallyAuthoriseNewMembers");
                settings.EmailAdminOnNewMemberSignup = forumRootNode.GetPropertyValue<bool>("emailAdminOnNewMemberSignup");
                settings.NewMembersMustConfirmAccountsViaEmail = forumRootNode.GetPropertyValue<bool>("newMembersMustConfirmAccountsViaEmail");
                settings.AllowMemberSignatures = forumRootNode.GetPropertyValue<bool>("allowMemberSignatures");
                settings.TopicsPerPage = forumRootNode.GetPropertyValue<int>("topicsPerPage");
                settings.AllowPostsToBeMarkedAsSolution = forumRootNode.GetPropertyValue<bool>("allowPostsToBeMarkedAsSolution");
                settings.PostsPerPage = forumRootNode.GetPropertyValue<int>("postsPerPage");
                settings.ActivitiesPerPage = forumRootNode.GetPropertyValue<int>("activitiesPerPage");
                settings.AllowPrivateMessages = forumRootNode.GetPropertyValue<bool>("allowPrivateMessages");
                settings.PrivateMessageInboxSize = forumRootNode.GetPropertyValue<int>("privateMessageInboxSize");
                settings.PrivateMessageFloodControl = forumRootNode.GetPropertyValue<int>("privateMessageFloodControl");

                // Points
                settings.AllowPoints = forumRootNode.GetPropertyValue<bool>("allowPoints");
                settings.AmountOfPointsBeforeAUserCanVote = forumRootNode.GetPropertyValue<int>("amountOfPointsBeforeAUserCanVote");
                settings.PointsAddedPerNewPost = forumRootNode.GetPropertyValue<int>("pointsAddedPerNewPost");
                settings.PointsAddedForPositiveVote = forumRootNode.GetPropertyValue<int>("pointsAddedForPositiveVote");
                settings.PointsDeductedForNegativeVote = forumRootNode.GetPropertyValue<int>("pointsDeductedForNegativeVote");
                settings.PointsAddedForASolution = forumRootNode.GetPropertyValue<int>("pointsAddedForASolution");

                // Email
                settings.AdminEmailAddress = forumRootNode.GetPropertyValue<string>("adminEmailAddress");
                settings.NotificationReplyEmailAddress = forumRootNode.GetPropertyValue<string>("notificationReplyEmailAddress");

                // Theme
                settings.Theme = forumRootNode.GetPropertyValue<string>("theme");

                // Member Group
                var memberGroupService = AppHelpers.UmbServices().MemberGroupService;
                var memberGroupCsv = forumRootNode.GetPropertyValue<string>("newMemberStartingGroup");

                //NOTE: Take the FIRST one only if there are multiple
                if (memberGroupCsv != null)
                {
                    var memberGroupId = memberGroupCsv.Split(',').FirstOrDefault();
                    settings.Group = memberGroupService.GetByName(memberGroupId);
                }
                else
                {
                    settings.Group = memberGroupService.GetByName(AppConstants.MemberGroupDefault);
                }

                // Spam
                settings.EnableAkismetSpamControl = forumRootNode.GetPropertyValue<bool>("enableAkismetSpamControl");
                settings.AkismetKey = forumRootNode.GetPropertyValue<string>("enterYourAkismetKeyHere");
                settings.SpamQuestion = forumRootNode.GetPropertyValue<string>("enterASpamRegistrationPreventionQuestion");
                settings.SpamAnswer = forumRootNode.GetPropertyValue<string>("enterTheAnswerToYourSpamQuestion");

                // Social
                settings.EnableSocialLogins = forumRootNode.GetPropertyValue<bool>("EnableSocialLogins");
                settings.FacebookAppId = forumRootNode.GetPropertyValue<string>("FacebookAppId");
                settings.FacebookAppSecret = forumRootNode.GetPropertyValue<string>("FacebookAppSecret");

                // Meta
                settings.PageTitle = forumRootNode.GetPropertyValue<string>("pageTitle");
                settings.MetaDescription = forumRootNode.GetPropertyValue<string>("metaDescription");

                // Umbraco Properties
                settings.LastModified = forumRootNode.UpdateDate;

                return settings;
            }
            return null;
        }

        public static DialogueSettings Settings(int nodeId)
        {
            if (!HttpContext.Current.Items.Contains(AppConstants.SiteSettingsKey))
            {
                var currentPage = AppHelpers.GetNode(nodeId);
                var forumNode = currentPage.AncestorOrSelf(AppConstants.DocTypeForumRoot);
                HttpContext.Current.Items.Add(AppConstants.SiteSettingsKey, Settings(forumNode));
            }
            return HttpContext.Current.Items[AppConstants.SiteSettingsKey] as DialogueSettings;
        }

        public static DialogueSettings Settings()
        {
            if (!HttpContext.Current.Items.Contains(AppConstants.SiteSettingsKey))
            {
                var currentPage = AppHelpers.CurrentPage();
                var forumNode = currentPage.AncestorOrSelf(AppConstants.DocTypeForumRoot);
                if (forumNode == null)
                {
                    // Only do this is if we can't find the forum normally
                    currentPage.DescendantOrSelf(AppConstants.DocTypeForumRoot);
                }
                HttpContext.Current.Items.Add(AppConstants.SiteSettingsKey, Settings(forumNode));
            }
            return HttpContext.Current.Items[AppConstants.SiteSettingsKey] as DialogueSettings;
        }
    }
}