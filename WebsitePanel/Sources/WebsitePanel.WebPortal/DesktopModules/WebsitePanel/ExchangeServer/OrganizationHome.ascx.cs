// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.HostedSolution;

namespace WebsitePanel.Portal.ExchangeServer
{
    public partial class OrganizationHome : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOrgStats();
            }

        }

        private void BindExchangeStats(bool hideItems, PackageContext cntx)
        {
            OrganizationStatistics exchangeOrgStats = ES.Services.ExchangeServer.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            OrganizationStatistics exchangeTenantStats = ES.Services.ExchangeServer.GetOrganizationStatistics(PanelRequest.ItemID);

            lnkMailboxes.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "mailboxes",
            "SpaceID=" + PanelSecurity.PackageId.ToString());


            lnkSharedMailboxes.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "mailboxes",
            "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkResourceMailboxes.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "mailboxes",
            "SpaceID=" + PanelSecurity.PackageId.ToString());


            lnkContacts.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "contacts",
            "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkLists.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "dlists",
            "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkExchangeStorage.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "storage_usage",
            "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkFolders.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "public_folders",
            "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkExchangeLitigationHold.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "storage_usage",
            "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkExchangeArchiving.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "archivingmailboxes",
            "SpaceID=" + PanelSecurity.PackageId.ToString());


            mailboxesStats.QuotaUsedValue = exchangeOrgStats.CreatedMailboxes;
            mailboxesStats.QuotaValue = exchangeOrgStats.AllocatedMailboxes;
            if (exchangeOrgStats.AllocatedMailboxes != -1) mailboxesStats.QuotaAvailable = exchangeTenantStats.AllocatedMailboxes - exchangeTenantStats.CreatedMailboxes;

            mailboxesSharedStats.QuotaUsedValue = exchangeOrgStats.CreatedSharedMailboxes;
            mailboxesSharedStats.QuotaValue = exchangeOrgStats.AllocatedSharedMailboxes;
            if (exchangeOrgStats.AllocatedSharedMailboxes != -1) mailboxesSharedStats.QuotaAvailable = exchangeTenantStats.AllocatedSharedMailboxes - exchangeTenantStats.CreatedSharedMailboxes;

            mailboxesResourceStats.QuotaUsedValue = exchangeOrgStats.CreatedResourceMailboxes;
            mailboxesResourceStats.QuotaValue = exchangeOrgStats.AllocatedResourceMailboxes;
            if (exchangeOrgStats.AllocatedResourceMailboxes != -1) mailboxesResourceStats.QuotaAvailable = exchangeTenantStats.AllocatedResourceMailboxes - exchangeTenantStats.CreatedResourceMailboxes;

            if (exchangeTenantStats.AllocatedContacts == 0) this.rowContacts.Style.Add("display", "none");
            else
            {
                contactsStats.QuotaUsedValue = exchangeOrgStats.CreatedContacts;
                contactsStats.QuotaValue = exchangeOrgStats.AllocatedContacts;
                if (exchangeOrgStats.AllocatedContacts != -1) contactsStats.QuotaAvailable = exchangeTenantStats.AllocatedContacts - exchangeTenantStats.CreatedContacts;
            }

            if (exchangeTenantStats.AllocatedDistributionLists == 0) this.rowLists.Style.Add("display", "none");
            else
            {
                listsStats.QuotaUsedValue = exchangeOrgStats.CreatedDistributionLists;
                listsStats.QuotaValue = exchangeOrgStats.AllocatedDistributionLists;
                if (exchangeOrgStats.AllocatedDistributionLists != -1) listsStats.QuotaAvailable = exchangeTenantStats.AllocatedDistributionLists - exchangeTenantStats.CreatedDistributionLists;
            }

            if (!hideItems)
            {
                exchangeStorageStats.QuotaUsedValue = exchangeOrgStats.UsedDiskSpace;
                exchangeStorageStats.QuotaValue = exchangeOrgStats.AllocatedDiskSpace;
                if (exchangeOrgStats.AllocatedDiskSpace != -1)
                {
                    exchangeStorageStats.QuotaAvailable = exchangeTenantStats.AllocatedDiskSpace - exchangeTenantStats.UsedDiskSpace;
                }
            }
            else
                this.rowExchangeStorage.Style.Add("display", "none");

            if (exchangeTenantStats.AllocatedPublicFolders == 0) this.rowFolders.Style.Add("display", "none");
            else
            {
                foldersStats.QuotaUsedValue = exchangeOrgStats.CreatedPublicFolders;
                foldersStats.QuotaValue = exchangeOrgStats.AllocatedPublicFolders;
                if (exchangeOrgStats.AllocatedPublicFolders != -1) foldersStats.QuotaAvailable = exchangeTenantStats.AllocatedPublicFolders - exchangeTenantStats.CreatedPublicFolders;
            }

            if ((!hideItems) && (Utils.CheckQouta(Quotas.EXCHANGE2007_ALLOWLITIGATIONHOLD, cntx)))
            {
                exchangeLitigationHoldStats.QuotaUsedValue = exchangeOrgStats.UsedLitigationHoldSpace;
                exchangeLitigationHoldStats.QuotaValue = exchangeOrgStats.AllocatedLitigationHoldSpace;
                if (exchangeOrgStats.AllocatedLitigationHoldSpace != -1)
                {
                    exchangeLitigationHoldStats.QuotaAvailable = exchangeTenantStats.AllocatedLitigationHoldSpace - exchangeTenantStats.UsedLitigationHoldSpace;
                }
            }
            else
                this.rowExchangeLitigationHold.Style.Add("display", "none");

            if (!hideItems)
            {
                exchangeArchivingStatus.QuotaUsedValue = exchangeOrgStats.UsedArchingStorage;
                exchangeArchivingStatus.QuotaValue = exchangeOrgStats.AllocatedArchingStorage;
                if (exchangeOrgStats.AllocatedArchingStorage != -1)
                {
                    exchangeArchivingStatus.QuotaAvailable = exchangeTenantStats.AllocatedArchingStorage - exchangeTenantStats.UsedArchingStorage;
                }
            }
            else
                this.rowExchangeArchiving.Style.Add("display", "none");

        }

        private void BindOrgStats()
        {
            Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            bool hideItems = false;

            UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

            if (user != null)
            {
                if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, cntx)))
                    hideItems = true;
            }



            lblOrganizationNameValue.Text = org.Name;
            lblOrganizationIDValue.Text = org.OrganizationId;
            lblCreatedValue.Text = org.CreatedDate.Date.ToShortDateString();

            OrganizationStatistics orgStats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            OrganizationStatistics tenantStats = ES.Services.Organizations.GetOrganizationStatistics(PanelRequest.ItemID);
            if (orgStats == null)
                return;

            if (!hideItems)
            {
                domainStats.QuotaUsedValue = orgStats.CreatedDomains;
                domainStats.QuotaValue = orgStats.AllocatedDomains;
                if (orgStats.AllocatedDomains != -1) domainStats.QuotaAvailable = tenantStats.AllocatedDomains - tenantStats.CreatedDomains;

                userStats.QuotaUsedValue = orgStats.CreatedUsers;
                userStats.QuotaValue = orgStats.AllocatedUsers;
                if (orgStats.AllocatedUsers != -1)
                    userStats.QuotaAvailable = tenantStats.AllocatedUsers - tenantStats.CreatedUsers;

                deletedUserStats.QuotaUsedValue = orgStats.DeletedUsers;
                deletedUserStats.QuotaValue = orgStats.AllocatedDeletedUsers;
                if (orgStats.AllocatedDeletedUsers != -1)
                    userStats.QuotaAvailable = tenantStats.AllocatedDeletedUsers - tenantStats.DeletedUsers;

                lnkDomains.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "domains",
                    "SpaceID=" + PanelSecurity.PackageId);

                lnkUsers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "users",
                    "SpaceID=" + PanelSecurity.PackageId);

                lnkDeletedUsers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "deleted_users",
                    "SpaceID=" + PanelSecurity.PackageId);

                if (Utils.CheckQouta(Quotas.ORGANIZATION_SECURITYGROUPS, cntx))
                {
                    securGroupsStat.Visible = true;

                    groupStats.QuotaUsedValue = orgStats.CreatedGroups;
                    groupStats.QuotaValue = orgStats.AllocatedGroups;
                    if (orgStats.AllocatedGroups != -1) groupStats.QuotaAvailable = tenantStats.AllocatedGroups - tenantStats.CreatedGroups;

                    lnkGroups.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "secur_groups",
                        "SpaceID=" + PanelSecurity.PackageId);
                }
                else
                {
                    securGroupsStat.Visible = false;
                }
            }
            else
                organizationStatsPanel.Visible = false;

            
            if (cntx.Groups.ContainsKey(ResourceGroups.Exchange))
            {
                exchangeStatsPanel.Visible = true;
                BindExchangeStats(hideItems, cntx);
            }
            else
                exchangeStatsPanel.Visible = false;


            //Show SharePoint statistics
            if (cntx.Groups.ContainsKey(ResourceGroups.SharepointFoundationServer))
            {
                sharePointStatsPanel.Visible = true;

                lnkSiteCollections.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "sharepoint_sitecollections",
                "SpaceID=" + PanelSecurity.PackageId);
                siteCollectionsStats.QuotaUsedValue = orgStats.CreatedSharePointSiteCollections;
                siteCollectionsStats.QuotaValue = orgStats.AllocatedSharePointSiteCollections;
                if (orgStats.AllocatedSharePointSiteCollections != -1) siteCollectionsStats.QuotaAvailable = tenantStats.AllocatedSharePointSiteCollections - tenantStats.CreatedSharePointSiteCollections;
            }
            else
                sharePointStatsPanel.Visible = false;

            //Show SharePoint statistics
            if (cntx.Groups.ContainsKey(ResourceGroups.SharepointEnterpriseServer))
            {
                sharePointEnterpriseStatsPanel.Visible = true;

                lnkEnterpriseSiteCollections.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "sharepoint_enterprise_sitecollections",
                "SpaceID=" + PanelSecurity.PackageId);
                enterpriseSiteCollectionsStats.QuotaUsedValue = orgStats.CreatedSharePointEnterpriseSiteCollections;
                enterpriseSiteCollectionsStats.QuotaValue = orgStats.AllocatedSharePointEnterpriseSiteCollections;
                if (orgStats.AllocatedSharePointEnterpriseSiteCollections != -1) enterpriseSiteCollectionsStats.QuotaAvailable = tenantStats.AllocatedSharePointEnterpriseSiteCollections - tenantStats.CreatedSharePointEnterpriseSiteCollections;
            }
            else
                sharePointEnterpriseStatsPanel.Visible = false;


            if (cntx.Groups.ContainsKey(ResourceGroups.OCS))
            {
                ocsStatsPanel.Visible = true;
                BindOCSStats(orgStats, tenantStats);
            }
            else
                ocsStatsPanel.Visible = false;

            if (cntx.Groups.ContainsKey(ResourceGroups.BlackBerry))
            {
                besStatsPanel.Visible = true;
                BindBESStats(orgStats, tenantStats);
            }
            else
                besStatsPanel.Visible = false;

            if (cntx.Groups.ContainsKey(ResourceGroups.Lync))
            {
                lyncStatsPanel.Visible = true;
                BindLyncStats(orgStats, tenantStats);
            }
            else
                lyncStatsPanel.Visible = false;



            if (org.CrmOrganizationId != Guid.Empty)
            {

                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                {
                    crm2013StatsPanel.Visible = true;
                    crmStatsPanel.Visible = false;
                    BindCRM2013Stats(orgStats, tenantStats);
                }
                else if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM))
                {
                    crmStatsPanel.Visible = true;
                    crm2013StatsPanel.Visible = false;
                    BindCRMStats(orgStats, tenantStats);
                }

            }
            else
            {
                crmStatsPanel.Visible = false;
                crm2013StatsPanel.Visible = false;
            }


            if (cntx.Groups.ContainsKey(ResourceGroups.EnterpriseStorage))
            {
                enterpriseStorageStatsPanel.Visible = true;
                BindEnterpriseStorageStats(orgStats, tenantStats);
            }
            else
                enterpriseStorageStatsPanel.Visible = false;

            if (cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels))
            {
                serviceLevelsStatsPanel.Visible = true;
                BindServiceLevelsStats(cntx);
            }
            else
                serviceLevelsStatsPanel.Visible = false;

            if (cntx.Groups.ContainsKey(ResourceGroups.RDS))
            {
                remoteDesktopStatsPanel.Visible = true;
                BindRemoteDesktopStats(orgStats, tenantStats);
            }
            else
            {
                remoteDesktopStatsPanel.Visible = false;
            }
        }

        private void BindCRMStats(OrganizationStatistics stats, OrganizationStatistics tenantStats)
        {
            lnkCRMUsers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "crmusers",
                "SpaceID=" + PanelSecurity.PackageId);

            lnkLimitedCRMUsers.NavigateUrl = lnkCRMUsers.NavigateUrl;
            lnkESSCRMUsers.NavigateUrl = lnkCRMUsers.NavigateUrl;

            lnkCRMDBSize.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "crm_storage_settings",
                "SpaceID=" + PanelSecurity.PackageId);

            crmUsersStats.QuotaUsedValue = stats.CreatedCRMUsers;
            crmUsersStats.QuotaValue = stats.AllocatedCRMUsers;

            //if (stats.AllocatedCRMUsers != -1) crmUsersStats.QuotaAvailable = tenantStats.AllocatedCRMUsers - tenantStats.CreatedCRMUsers;

            crmLimitedUsersStats.QuotaUsedValue = stats.CreatedLimitedCRMUsers;
            crmLimitedUsersStats.QuotaValue = stats.AllocatedLimitedCRMUsers;

            crmESSUsersStats.QuotaUsedValue = stats.CreatedESSCRMUsers;
            crmESSUsersStats.QuotaValue = stats.AllocatedESSCRMUsers;

            crmDBSize.QuotaUsedValue = Convert.ToInt32(stats.UsedCRMDiskSpace > 0 ? stats.UsedCRMDiskSpace / (1024 * 1024) : -1);
            crmDBSize.QuotaValue = Convert.ToInt32(stats.AllocatedCRMDiskSpace>0 ? stats.AllocatedCRMDiskSpace/(1024*1024) : -1);
        }

        private void BindCRM2013Stats(OrganizationStatistics stats, OrganizationStatistics tenantStats)
        {
            lnkProfessionalCRMUsers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "crmusers",
                "SpaceID=" + PanelSecurity.PackageId);

            lnkBasicCRMUsers.NavigateUrl = lnkCRMUsers.NavigateUrl;
            lnkEssentialCRMUsers.NavigateUrl = lnkCRMUsers.NavigateUrl;

            lnkCRM2013DBSize.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "crm_storage_settings",
                "SpaceID=" + PanelSecurity.PackageId);

            crmProfessionalUsersStats.QuotaUsedValue = stats.CreatedProfessionalCRMUsers;
            crmProfessionalUsersStats.QuotaValue = stats.AllocatedProfessionalCRMUsers;

            crmBasicUsersStats.QuotaUsedValue = stats.CreatedBasicCRMUsers;
            crmBasicUsersStats.QuotaValue = stats.AllocatedBasicCRMUsers;

            crmEssentialUsersStats.QuotaUsedValue = stats.CreatedEssentialCRMUsers;
            crmEssentialUsersStats.QuotaValue = stats.AllocatedEssentialCRMUsers;

            crm2013DBSize.QuotaUsedValue = Convert.ToInt32(stats.UsedCRMDiskSpace > 0 ? stats.UsedCRMDiskSpace / (1024 * 1024) : -1);
            crm2013DBSize.QuotaValue = Convert.ToInt32(stats.AllocatedCRMDiskSpace > 0 ? stats.AllocatedCRMDiskSpace / (1024 * 1024) : -1);
        }

        private void BindOCSStats(OrganizationStatistics stats, OrganizationStatistics tenantStats)
        {
            ocsUsersStats.QuotaValue = stats.AllocatedOCSUsers;
            ocsUsersStats.QuotaUsedValue = stats.CreatedOCSUsers;
            if (stats.AllocatedOCSUsers != -1) ocsUsersStats.QuotaAvailable = tenantStats.AllocatedOCSUsers - tenantStats.CreatedOCSUsers;

            lnkOCSUsers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "ocs_users",
            "SpaceID=" + PanelSecurity.PackageId.ToString());
        }

        private void BindLyncStats(OrganizationStatistics stats, OrganizationStatistics tenantStats)
        {
            lyncUsersStats.QuotaValue = stats.AllocatedLyncUsers;
            lyncUsersStats.QuotaUsedValue = stats.CreatedLyncUsers;
            if (stats.AllocatedLyncUsers != -1) lyncUsersStats.QuotaAvailable = tenantStats.AllocatedLyncUsers - tenantStats.CreatedLyncUsers;

            lnkLyncUsers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "lync_users",
            "SpaceID=" + PanelSecurity.PackageId.ToString());
        }


        private void BindBESStats(OrganizationStatistics stats, OrganizationStatistics tenantStats)
        {
            besUsersStats.QuotaValue = stats.AllocatedBlackBerryUsers;
            besUsersStats.QuotaUsedValue = stats.CreatedBlackBerryUsers;
            if (stats.AllocatedBlackBerryUsers != -1) besUsersStats.QuotaAvailable = tenantStats.AllocatedBlackBerryUsers - tenantStats.CreatedBlackBerryUsers;

            lnkBESUsers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "blackberry_users",
            "SpaceID=" + PanelSecurity.PackageId.ToString());
        }

        private void BindEnterpriseStorageStats(OrganizationStatistics stats, OrganizationStatistics tenantStats)
        {
            enterpriseStorageSpaceStats.QuotaValue = stats.AllocatedEnterpriseStorageSpace;
            enterpriseStorageSpaceStats.QuotaUsedValue = stats.UsedEnterpriseStorageSpace;
            if (stats.AllocatedEnterpriseStorageSpace != -1) enterpriseStorageSpaceStats.QuotaAvailable = tenantStats.AllocatedEnterpriseStorageSpace - tenantStats.UsedEnterpriseStorageSpace;

            lnkEnterpriseStorageSpace.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "enterprisestorage_folders",
            "SpaceID=" + PanelSecurity.PackageId.ToString());

            enterpriseStorageFoldersStats.QuotaValue = stats.AllocatedEnterpriseStorageFolders;
            enterpriseStorageFoldersStats.QuotaUsedValue = stats.CreatedEnterpriseStorageFolders;
            if (stats.AllocatedEnterpriseStorageFolders != -1) enterpriseStorageFoldersStats.QuotaAvailable = tenantStats.AllocatedEnterpriseStorageFolders - tenantStats.CreatedEnterpriseStorageFolders;

            lnkEnterpriseStorageFolders.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "enterprisestorage_folders",
            "SpaceID=" + PanelSecurity.PackageId.ToString());
        }

        private void BindServiceLevelsStats(PackageContext cntx)
        {
            WebsitePanel.EnterpriseServer.Base.HostedSolution.ServiceLevel[] serviceLevels = ES.Services.Organizations.GetSupportServiceLevels();
            OrganizationUser[] accounts = ES.Services.Organizations.SearchAccounts(PanelRequest.ItemID, "", "", "", true);

            foreach (var quota in Array.FindAll<QuotaValueInfo>(
                    cntx.QuotasArray, x => x.QuotaName.Contains(Quotas.SERVICE_LEVELS)))
            {
                HtmlTableRow tr = new HtmlTableRow();
                    tr.Attributes["class"] = "OrgStatsRow";
                HtmlTableCell col1 = new HtmlTableCell();
                    col1.Attributes["class"] = "OrgStatsQuota";
                    col1.Attributes["nowrap"] = "nowrap";
                HyperLink link = new HyperLink();
                link.ID = "lnk_" + quota.QuotaName.Replace(Quotas.SERVICE_LEVELS, "").Replace(" ", string.Empty).Trim();
                    link.Text = quota.QuotaDescription.Replace(", users", " (users):");

                    col1.Controls.Add(link);

                    int levelId = serviceLevels.Where(x => x.LevelName == quota.QuotaName.Replace(Quotas.SERVICE_LEVELS, "")).FirstOrDefault().LevelId;
                    int usedInOrgCount = accounts.Where(x => x.LevelId == levelId).Count();

                HtmlTableCell col2 = new HtmlTableCell();
                QuotaViewer quotaControl = (QuotaViewer)LoadControl("../UserControls/QuotaViewer.ascx");
                    quotaControl.ID = quota.QuotaName.Replace(Quotas.SERVICE_LEVELS, "").Replace(" ", string.Empty).Trim() + "Stats";
                    quotaControl.QuotaTypeId = quota.QuotaTypeId;
                    quotaControl.DisplayGauge = true;
                    quotaControl.QuotaValue = quota.QuotaAllocatedValue;
                    quotaControl.QuotaUsedValue = usedInOrgCount;
                    //quotaControl.QuotaUsedValue = quota.QuotaUsedValue;
                    if (quota.QuotaAllocatedValue != -1) 
                        quotaControl.QuotaAvailable = quota.QuotaAllocatedValue - quota.QuotaUsedValue;

                    col2.Controls.Add(quotaControl);


                tr.Controls.Add(col1);
                tr.Controls.Add(col2);
                serviceLevelsStatsPanel.Controls.Add(tr);
            }
        }

        private void BindRemoteDesktopStats(OrganizationStatistics stats, OrganizationStatistics tenantStats)
        {
            rdsServers.QuotaValue = stats.AllocatedRdsServers;
            rdsServers.QuotaUsedValue = stats.CreatedRdsServers;
            if (stats.AllocatedRdsServers != -1)
            {
                rdsServers.QuotaAvailable = tenantStats.AllocatedRdsServers - tenantStats.CreatedRdsServers;
            }

            rdsCollections.QuotaValue = stats.AllocatedRdsCollections;
            rdsCollections.QuotaUsedValue = stats.CreatedRdsCollections;

            if (stats.AllocatedRdsCollections != -1)
            {
                rdsCollections.QuotaAvailable = tenantStats.AllocatedRdsCollections - tenantStats.CreatedRdsCollections;
            }                       

            rdsUsers.QuotaValue = stats.AllocatedRdsUsers;
            rdsUsers.QuotaUsedValue = stats.CreatedRdsUsers;

            if (stats.AllocatedRdsCollections != -1)
            {
                rdsUsers.QuotaAvailable = tenantStats.AllocatedRdsUsers - tenantStats.CreatedRdsUsers;
            }

            lnkRdsServers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId);
            lnkRdsCollections.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId);
            lnkRdsUsers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId);
        }
    }
}
