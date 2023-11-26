using Contractor.EntityFrameworkCore;
using Contractor.GeneralEntities;
using Contractor.Identities;
using Contractor.Subscriptions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Contractor.Identities
{
    public class IdentitySeeder
    {

        internal static void SeedData(IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var _roleManager = (RoleManager)scope.ServiceProvider.GetService(typeof(IRoleManager));
                var _userManager = (UserManager)scope.ServiceProvider.GetService(typeof(IUserManager));
                var _subscriptionRepository = (SubscriptionRepository)scope.ServiceProvider.GetService(typeof(ISubscriptionRepository));
                var _companyRepository = (Repository<Company, int>)scope.ServiceProvider.GetService(typeof(IRepository<Company, int>));
                 
                var dbcontext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var pages = SeedPages(dbcontext);

                var roles = SeedRoles(_roleManager, pages);

                var users = SeedUsers(_userManager, _roleManager, _subscriptionRepository, _companyRepository);
            }


        }

        private static List<Role> SeedRoles(RoleManager roleManager, List<Page> pages)
        {

            foreach (var roleName in RoleNames.Roles)
            {
                switch(roleName)
                {
                    case RoleNames.Admin:
                        CheckRole(roleManager, pages, RoleNames.Admin, PageNames.AllPages, FunctionalityNames.AllFunctionalities);
                        break;

                    case RoleNames.Contractor:
                        CheckRole(roleManager, pages, RoleNames.Contractor, PageNames.ContractorPages, FunctionalityNames.ContractorFunctionalities);
                        break;

                    case RoleNames.Consultant:
                        CheckRole(roleManager, pages, RoleNames.Consultant, PageNames.ConsultantPages, FunctionalityNames.ConsultantFunctionalities);
                        break;

                    case RoleNames.Owner:
                        CheckRole(roleManager, pages, RoleNames.Owner, PageNames.OwnerPages, FunctionalityNames.OwnerFunctionalities);
                        break;

                    default: 
                        break;

                }

            }

            return roleManager.Roles.ToList();


        }


        private static void CheckRole(RoleManager roleManager, List<Page> pages, string roleName, List<string> pageNameList, List<string> functionalityList)
        {
            var role = roleManager.GetRoleByNameAsync(roleName).GetAwaiter().GetResult();
            
            var pageClaim = pages.Where(x => pageNameList.Contains(x.Name))
                                .Select(x => new Tuple<int, string>(x.Id, x.Name))
                                .ToList();
            if (role == null)
            {
                role = Role.Create(roleName, true);

                role.AddPageClaims(pageClaim);

                role.AddFunctionalityClaims(functionalityList);

                _ = roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }
            else
            {
                role.UpdatePageClaims(pageClaim);

                role.UpdateFunctionalityClaims(functionalityList);

                _ = roleManager.UpdateRoleAsync(role).GetAwaiter().GetResult();
            }
        }

        private static List<User> SeedUsers(UserManager _userManager, RoleManager _roleManager , SubscriptionRepository subscriptionRepository, Repository<Company, int> companyRepository)
        {
            if (!_userManager.Users.Any())
            {
                foreach (var userItem in SeedUserNames.Users)
                {
                    var email = userItem.Item1;
                    var userType = userItem.Item2;
                    
                    var company = Company.Create();
                    companyRepository.AddAsync(company).GetAwaiter().GetResult();

                    var user = User.Create(email, email, null, userType, company, string.Empty);

                    var subscription =  subscriptionRepository.GetDefaultAsync().GetAwaiter().GetResult();
                    user.UpdateSubscriptionPlanId(subscription.Id);


                    var res = _userManager.CreateAsync(user, "P@ssw0rd").GetAwaiter().GetResult();

                    if (res.Succeeded)
                    {
                        switch (email)
                        {
                            case SeedUserNames.Admin:
                                var adminRole = _roleManager.FindByNameAsync(RoleNames.Admin).GetAwaiter().GetResult();
                                if (adminRole != null)
                                {
                                    _ = _userManager.AddToRoleAsync(user, RoleNames.Admin).GetAwaiter().GetResult();
                                }
                                break;

                            case SeedUserNames.Consultant:
                                var clientRole = _roleManager.FindByNameAsync(RoleNames.Consultant).GetAwaiter().GetResult();

                                if (clientRole != null)
                                {
                                    _ = _userManager.AddToRoleAsync(user, clientRole.Name).GetAwaiter().GetResult();
                                }
                                break;

                            case SeedUserNames.Contractor:
                                var contractorRole = _roleManager.FindByNameAsync(RoleNames.Contractor).GetAwaiter().GetResult();

                                if (contractorRole != null)
                                {
                                    _ = _userManager.AddToRoleAsync(user, contractorRole.Name).GetAwaiter().GetResult();
                                }
                                break;

                            case SeedUserNames.Owner:
                                var ownerRole = _roleManager.FindByNameAsync(RoleNames.Owner).GetAwaiter().GetResult();

                                if (ownerRole != null)
                                {
                                    _ = _userManager.AddToRoleAsync(user, ownerRole.Name).GetAwaiter().GetResult();
                                }
                                break;
                        }
                    }

                }
            }

            //this code is when we need to add the users to there roles
            //foreach (var user in _userManager.Users)
            //{
            //    switch (user.UserType)
            //    {
            //        case UserTypes.Admin:
            //            _ = _userManager.AddToRoleAsync(user, RoleNames.Admin).GetAwaiter().GetResult();
            //            break;
            //        case UserTypes.Owner:
            //            _ = _userManager.AddToRoleAsync(user, RoleNames.Owner).GetAwaiter().GetResult();
            //            break;
            //        case UserTypes.Consultant:
            //            _ = _userManager.AddToRoleAsync(user, RoleNames.Consultant).GetAwaiter().GetResult();
            //            break;
            //        case UserTypes.Contractor:
            //            _ = _userManager.AddToRoleAsync(user, RoleNames.Contractor).GetAwaiter().GetResult();
            //            break;

            //    }
            //}

            return _userManager.Users.ToList();


        }

        private static List<Page> SeedPages(DatabaseContext context)
        {
            var dbPageCount = context.Pages.Count();

            if (dbPageCount == 0)
            {
                var pages = Page.Create(PageNames.AllPages, null);

                context.Pages.AddRange(pages);

                context.SaveChanges();
            }
            else if (dbPageCount < PageNames.AllPages.Count)
            {
                foreach (var pageName in PageNames.AllPages)
                {
                    var page = context.Pages.FirstOrDefault(x => x.Name == pageName);

                    if (page == null)
                    {
                        page = Page.Create(pageName, null);

                        context.Pages.Add(page);
                    }
                }

                context.SaveChanges();
            }

            return context.Pages.ToList();
        }
    }
}
