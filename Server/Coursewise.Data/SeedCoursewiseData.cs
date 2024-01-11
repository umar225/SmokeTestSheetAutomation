using Coursewise.Common.Models;
using Coursewise.Common.Utilities;
using Coursewise.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data
{
    public static class SeedCoursewiseData
    {
        public static async Task SeedAllData(IServiceProvider serviceProvider, string env, IConfiguration configuration)
        {
            var context = serviceProvider.CreateScope().ServiceProvider.GetService<CoursewiseDbContext>()!;
            UserManager<CoursewiseUser> userManager = serviceProvider.CreateScope().ServiceProvider.GetService<UserManager<CoursewiseUser>>()!;
            if (!context.Courses.Any())
            {
                var categories = new List<Category>();
                var pmpCategory = new Category
                {
                    Name = "Project Management Qualifications",
                    DisplayName = "Project Management",
                    IsVisible = true
                };
                var coachingCategory = new Category
                {
                    Name = "Coaching Qualifications",
                    DisplayName = "Coaching",
                    IsVisible = true
                };
                var leadershipCategory = new Category
                {
                    Name = "Leadership and Management Qualifications",
                    DisplayName = "Leadership and Management",
                    IsVisible = true
                };
                var nedCategory = new Category
                {
                    Name = "NED Training",
                    DisplayName = "NED Training",
                    IsVisible = true
                };
                categories.Add(pmpCategory);
                categories.Add(coachingCategory);
                categories.Add(leadershipCategory);
                categories.Add(nedCategory);
                await context.Categories.AddRangeAsync(categories);

                #region Level
                var levels = new List<Level>();
                var directorLevel = new Level
                {
                    Name = "Director +"
                };
                var midManagerLevel = new Level
                {
                    Name = "Mid-level Manager"
                };
                var seniorMangerLevel = new Level
                {
                    Name = "Senior-level Manager"
                };
                var entryLevel = new Level
                {
                    Name = "Entry-level"
                };
                var supervisorLevel = new Level
                {
                    Name = "Supervisor"
                };
                levels.Add(directorLevel);
                levels.Add(midManagerLevel);
                levels.Add(seniorMangerLevel);
                levels.Add(entryLevel);
                levels.Add(supervisorLevel);
                await context.Levels.AddRangeAsync(levels);
                #endregion

                #region Courses
                // pmp
                var courses = new List<Course>();
                courses.Add(new Course
                {
                    CategoryId = pmpCategory.Id,
                    Name = "Project Management Professional (PMP)",
                    Price = 420,
                    Desription = "<div><b>Learning Objectives</b><p>At the conclusion of the course students will:</p><ul><li>Have sufficient knowledge and understanding to work as an informed member of a project team undertaking a variety of project management roles</li><li>Be able to examine and analyse the inputs, tools and techniques of the processes and knowledge areas of the PMBOK Guide</li><li>Be able to describe each process group and knowledge area of the PMBOK Guide</li><li>12 months online access to our PMP course</li><li>Full tutor support</li><li>PMP exam simulator</li></ul></div><div class='customDes'><b>Optional extra</b><p></p><p>The PMBOK Guide Sixth Edition and Agile Practice Guide. When you order the PMBOK Guide Sixth Edition (in English) you will also receive a complimentary copy of the Agile Practice Guide.</p></div>",
                    ProviderName = "ILX Group",
                    ProviderPrice = 504,
                });
                courses.Add(new Course
                {
                    CategoryId = pmpCategory.Id,
                    Name = "PGMP",
                    Price = 420,
                    Desription = "<b>Learning Objectives</b><p>By the end of this course you will be able to:</p><ul><li>Develop a strong leadership approach to managing multiple projects and navigating complex activities</li><li>Become an efficient programme manager with a good understanding of business strategy and programme processes</li><li>Use effective tools and techniques, including balanced scorecard, programme roadmapping and project selection criteria</li><li>Acquire the relevant knowledge and skills required to pass the PgMP certification exam</li></ul><b>WHAT'S COVERED?</b><p>You will learn PMIs five programme management performance domains:</p><ul><li>Programme strategy alignment</li><li>Lifecycle management</li><li>Stakeholder engagement</li><li>Benefits management</li><li>Governance</li></ul><p>The following modules will be covered:</p><ul><li>Overview of the PMI-PgMP course</li><li>Introduction to programme management</li><li>Programme management performance domains</li><li>Strategy alignment</li><li>Benefits management</li><li>Stakeholder engagement</li><li>Governance</li><li>Life cycle management</li><li>Supporting processes: definition phase, benefits delivery phase and closure phase</li></ul>",
                    ProviderName = "ILX Group",
                    ProviderPrice = 504,
                });
                courses.Add(new Course
                {
                    CategoryId = pmpCategory.Id,
                    Name = "Prince 2 Foundation and practitioner",
                    Price = 1380,
                    Desription = "<b>Learning Objectives</b><p>By the end of the PRINCE2 Foundation and Practitioner e-learning courses you should be able to:</p><ul><li>Understand key concepts relating to projects and PRINCE2</li><li>Understand how the PRINCE2 principles underpin the PRINCE2 method</li><li>Understand the PRINCE2 themes and how they are applied throughout the project</li><li>Understand the PRINCE2 processes and how they are carried out throughout the project</li><li>Apply the PRINCE2 principles in context</li><li>Apply and tailor relevant aspects of PRINCE2 themes in context</li><li>Apply and tailor relevant aspects of PRINCE2 processes in context</li></ul>",
                    ProviderName = "ILX Group",
                    ProviderPrice = 1554,
                });
                // cocahing
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    Name = "ILM Level 7 Diploma in Coaching and Mentoring",
                    CourseLevels = new List<CourseLevel>() { new CourseLevel { Level = directorLevel } },
                    Price = 0,
                    Desription = @"<div>
                                      <b class='d-block mb-2'>What is it?</b>
                                      <p>
                                        The ILM Level 7 Diploma provides delegates with the required knowledge and
                                        skills to mentor and coach others at a senior level. You will:
                                      </p>
                                      <ul class='mb-4'>
                                        <li>Further develop your performance as managers/coaches</li>
                                        <li>
                                          Understand how to manage or coach senior leaders in an organisational
                                          setting
                                        </li>
                                        <li>Understand how coaching and mentoring impacts an organisation.</li>
                                      </ul>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>Who’s it for?</b>
                                      <p class='mb-4'>
                                        This course is aimed at participants who will be significantly involved in
                                        executive coaching/leadership mentoring within an organisation or more
                                        likely as a freelancer.
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How long will it take?</b>
                                      <p class='mb-4'>On average it will take 9-12 months.</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How is it assessed?</b>
                                      <p>Assessed by approved ILM Supervisors/markers</p>
                                      <p class='mb-4'>
                                        Generally you will work with your provider to get the written and practical
                                        assessment completed to ILM standards - depending on which level you choose
                                        will determine the amount of assessments - some can just be written essays
                                        and some can include supervised coaching sessions. Once your provider is
                                        happy with your standard of work it will be sent to the ILM to assess in
                                        house.
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>What qualification will I receive?</b>
                                      <p class='mb-4'>You will receive a Level 7 Diploma in Coaching and Mentoring.</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How much will it cost?</b>
                                      <p>
                                        £1,800 - £6,500
                                      </p>
                                    </div>"
                });
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    Name = "ILM Level 7 Certificate in Coaching and Mentoring",
                    CourseLevels = new List<CourseLevel>() { new CourseLevel { Level = directorLevel } },
                    Price = 0,
                    Desription = @"<div>
                                      <b class='d-block mb-2'>What is it?</b>
                                      <p>
                                        The ILM Level 7 Certificate in Coaching and Mentoring is the highest
                                        coaching certificate achievable in the UK. It is equal to a postgraduate
                                        degree in which you will:
                                      </p>
                                      <ul class='mb-4'>
                                        <li>Be able to construct organisational mentoring and coaching policies</li>
                                        <li>
                                          Demonstrate how impact, support and recognition of coaching and mentoring
                                          is accepted in an organisation
                                        </li>
                                        <li>Understand how coaching and mentoring impacts an organisation.</li>
                                      </ul>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>Who’s it for?</b>
                                      <p class='mb-4'>
                                        This course is aimed at participants who will be significantly involved in
                                        executive coaching/leadership mentoring within an organisation or more
                                        likely as a freelancer.
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How long will it take?</b>
                                      <p class='mb-4'>On average it will take 12 months.</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How is it assessed?</b>
                                      <p>Assessed by approved ILM Supervisors/markers</p>
                                      <p class='mb-4'>
                                        Generally you will work with your provider to get the written and practical
                                        assessment completed to ILM standards - depending on which level you choose
                                        will determine the amount of assessments - some can just be written essays
                                        and some can include supervised coaching sessions. Once your provider is
                                        happy with your standard of work it will be sent to the ILM to assess in
                                        house.
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>What qualification will I receive?</b>
                                      <p>You will receive a Level 7 Certificate in Coaching and Mentoring.</p>
                                      <p class='mb-4'>The ILM level 7 Certificate is the equivalent of a postgraduate degree</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How much will it cost?</b>
                                      <p>
                                        £1,650 - £5,000
                                      </p>
                                    </div>"
                });
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    CourseLevels = new List<CourseLevel>() { new CourseLevel { Level = directorLevel } },
                    Name = "Transition Excellence",
                    Price = 0,
                    Desription = @"<div>
                                      <b class='d-block mb-2'>What is it?</b>
                                      <p class='mb-4'>
                                        This is a corporate coaching qualification accredited by the ICF (International Coaching Federation)
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>Who’s it for?</b>
                                      <p class='mb-4'>
                                        High-performing leaders in the corporate world are burned out and tired. C-suite executives are looking for a way out. Coaching is a way for them to make a great deal of income and a huge impact, on their own terms. And be happier! Yet, statistics indicate that at least 80% of coaches make £20K a year or less... They spend tens of thousands of pounds on coach training, branding, and marketing, expensive websites, or search engine optimization. And then they get no clients… The top 4% of coaches consistently make between £200K – £1M a year, coaching extraordinary clients, a few days a month. And that’s where Transition Excellence comes in. You do not need to learn how to get clients online using social media, SEO, Google Adwords, Facebook ads, or email lists and you do not need ‘experts’ telling you to build a website, start a campaign, or learn the latest ad technology. Who wants more soul-sucking networking meetings?! You will be able to, however… create clients by serving people so powerfully that they never forget your conversation – for the rest of their life. You do want to 10X your clients’ results. You’d love to make £100K, £200K, £500K a year – by word-of-mouth. And you’d love to coach a few days a week, a few weeks a month. During Transition Excellence you will learn how to coach your own clients powerfully. Build a sense of deep, natural confidence. Learn extraordinary, world-class coaching skills. Join a community of high-level leaders and coaches.
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How long will it take?</b>
                                      <p class='mb-4'>100 days</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How is it assessed?</b>
                                      <p class='mb-4'>Practical & Written assignment</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>What qualification will I receive?</b>
                                      <p class='mb-4'>ICF Accreditation</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How much will it cost?</b>
                                      <p>
                                        £6,050
                                      </p>
                                    </div>"
                });
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    CourseLevels = new List<CourseLevel>() { new CourseLevel { Level = midManagerLevel },
                        new CourseLevel { Level = supervisorLevel },
                        new CourseLevel { Level = entryLevel },
                        new CourseLevel { Level = seniorMangerLevel },
                    },
                    Name = "ILM Level 5 Certificate in Coaching - Effective Coaching and Mentoring",
                    Price = 0,
                    Desription = @"<div>
                                    <b class='d-block mb-2'>What is it?</b>
                                    <p>
                                    The ILM Level 5 in Effective Coaching and Mentoring is professional
                                    qualification equivalent to a university foundation degree. You will:
                                    </p>
                                    <ul class='mb-4'>
                                        <li>
                                            Learn how to manage the coaching and mentoring process within an
                                            organisation
                                        </li>
                                        <li>
                                            Deepen your understanding of how the workplace can affect coaching and
                                            mentoring
                                        </li>
                                        <li>Plan your future development in coaching and mentoring.</li>
                                    </ul>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>Who’s it for?</b>
                                    <p class='mb-4'>
                                    This course is ideal for those with responsibility for coaching and
                                    mentoring others in the workplace such as a manager or team leader. It is
                                    also perfect for people interested in embarking upon a career as a
                                    professional coach.
                                    </p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How long will it take?</b>
                                    <p class='mb-4'>On average it will take 10 – 12 months.</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How is it assessed?</b>
                                    <p>Assessed by approved ILM Supervisors/markers</p>
                                    <p class='mb-4'>
                                    Generally you will work with your provider to get the written and practical
                                    assessment completed to ILM standards – depending on which level you choose
                                    will determine the amount of assessments – some can just be written essays
                                    and some can include supervised coaching sessions. Once your provider is
                                    happy with your standard of work it will be sent to the ILM to assess in
                                    house.
                                    </p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>What qualification will I receive?</b>
                                    <p>You will receive a Level 5 Certificate in Coaching and Mentoring (QCF).</p>
                                    <p class='mb-4'>The ILM level 5 is the equivalent of a foundation degree.</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How much will it cost?</b>
                                    <p>£900 – £2,900</p>
                                </div>"
                });
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    Name = "Transition Excellence",
                    ProviderName = "Rich Litvin",
                    Price = 6000,
                    Desription = "<b>What is it?</b><p>This is a corporate coaching qualification accredited by the ICF (International Coaching Federation)</p><b>Who’s it for?</b><p>High-performing leaders in the corporate world are burned out and tired. C-suite executives are looking for a way out. Coaching is a way for them to make a great deal of income and a huge impact, on their own terms. And be happier! Yet, statistics indicate that at least 80% of coaches make £20K a year or less... They spend tens of thousands of pounds on coach training, branding, and marketing, expensive websites, or search engine optimization. And then they get no clients… The top 4% of coaches consistently make between £200K – £1M a year, coaching extraordinary clients, a few days a month. And that’s where Transition Excellence comes in. You do not need to learn how to get clients online using social media, SEO, Google Adwords, Facebook ads, or email lists and you do not need ‘experts’ telling you to build a website, start a campaign, or learn the latest ad technology. Who wants more soul-sucking networking meetings?! You will be able to, however… create clients by serving people so powerfully that they never forget your conversation – for the rest of their life. You do want to 10X your clients’ results. You’d love to make £100K, £200K, £500K a year – by word-of-mouth. And you’d love to coach a few days a week, a few weeks a month. During Transition Excellence you will learn how to coach your own clients powerfully. Build a sense of deep, natural confidence. Learn extraordinary, world-class coaching skills. Join a community of high-level leaders and coaches.</p><b>How long will it take?</b><p>100 days</p><b>How is it assessed?</b><p>Practical & Written assignment</p><b>What qualification will I receive?</b><p>ICF Accreditation</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    Name = "ILM 7 Diploma in Coaching and Mentoring",
                    ProviderName = "Academy of Leadership & Management",
                    Price = 3420,
                    Desription = "<b>What is it?</b><p>The ILM Level 7 Diploma provides delegates with the required knowledge and skills to mentor and coach others at a senior level. You will:</p><ul><li>Further develop your performance as managers/coaches</li><li>Understand how to manage or coach senior leaders in an organisational setting</li><li>Understand how coaching and mentoring impacts an organisation.</li></ul><b>Who’s it for?</b><p>This course is aimed at participants who will be significantly involved in executive coaching/leadership mentoring within an organisation or more likely as a freelancer.</p><b>How long will it take?</b><p>On average it will take 9-12 months.</p><b>How is it assessed?</b><p>Assessed by approved ILM Supervisors/markers</p><p>Generally you will work with your provider to get the written and practical assessment completed to ILM standards - depending on which level you choose will determine the amount of assessments - some can just be written essays and some can include supervised coaching sessions. Once your provider is happy with your standard of work it will be sent to the ILM to assess in house.</p><b>What qualification will I receive?</b><p>You will receive a Level 7 Diploma in Coaching and Mentoring.</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    Name = "ILM 7 Diploma in Coaching and Mentoring",
                    ProviderName = "UK College of Personal Development",
                    Price = 2594.40,
                    Desription = "<b>What is it?</b><p>The ILM Level 7 Diploma provides delegates with the required knowledge and skills to mentor and coach others at a senior level. You will:</p><ul><li>Further develop your performance as managers/coaches</li><li>Understand how to manage or coach senior leaders in an organisational setting</li><li>Understand how coaching and mentoring impacts an organisation.</li></ul><b>Who’s it for?</b><p>This course is aimed at participants who will be significantly involved in executive coaching/leadership mentoring within an organisation or more likely as a freelancer.</p><b>How long will it take?</b><p>On average it will take 9-12 months.</p><b>How is it assessed?</b><p>Assessed by approved ILM Supervisors/markers</p><p>Generally you will work with your provider to get the written and practical assessment completed to ILM standards - depending on which level you choose will determine the amount of assessments - some can just be written essays and some can include supervised coaching sessions. Once your provider is happy with your standard of work it will be sent to the ILM to assess in house.</p><b>What qualification will I receive?</b><p>You will receive a Level 7 Diploma in Coaching and Mentoring.</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    Name = "ILM 7 Certificate in Coaching and Mentoring",
                    ProviderName = "Academy of Leadership & Management",
                    Price = 2520,
                    Desription = "<b>What is it?</b><p>The ILM Level 7 Certificate in Coaching and Mentoring is the highest coaching certificate achievable in the UK. It is equal to a postgraduate degree in which you will:</p><ul><li>Be able to construct organisational mentoring and coaching policies</li><li>Demonstrate how impact, support and recognition of coaching and mentoring is accepted in an organisation</li><li>Understand how coaching and mentoring impacts an organisation.</li></ul><b>Who’s it for?</b><p>This course is aimed at participants who will be significantly involved in executive coaching/leadership mentoring within an organisation or more likely as a freelancer.</p><b>How long will it take?</b><p>On average it will take 12 months.</p><b>How is it assessed?</b><p>Assessed by approved ILM Supervisors/markers</p><p>Generally you will work with your provider to get the written and practical assessment completed to ILM standards - depending on which level you choose will determine the amount of assessments - some can just be written essays and some can include supervised coaching sessions. Once your provider is happy with your standard of work it will be sent to the ILM to assess in house.</p><b>What qualification will I receive?</b><p>You will receive a Level 7 Certificate in Coaching and Mentoring.</p><p>The ILM level 7 Certificate is the equivalent of a postgraduate degree</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    Name = "ILM 7 Certificate in Coaching and Mentoring",
                    ProviderName = "UK College of Personal Development",
                    Price = 2395,
                    Desription = "<b>What is it?</b><p>The ILM Level 7 Certificate in Coaching and Mentoring is the highest coaching certificate achievable in the UK. It is equal to a postgraduate degree in which you will:</p><ul><li>Be able to construct organisational mentoring and coaching policies</li><li>Demonstrate how impact, support and recognition of coaching and mentoring is accepted in an organisation</li><li>Understand how coaching and mentoring impacts an organisation.</li></ul><b>Who’s it for?</b><p>This course is aimed at participants who will be significantly involved in executive coaching/leadership mentoring within an organisation or more likely as a freelancer.</p><b>How long will it take?</b><p>On average it will take 12 months.</p><b>How is it assessed?</b><p>Assessed by approved ILM Supervisors/markers</p><p>Generally you will work with your provider to get the written and practical assessment completed to ILM standards - depending on which level you choose will determine the amount of assessments - some can just be written essays and some can include supervised coaching sessions. Once your provider is happy with your standard of work it will be sent to the ILM to assess in house.</p><b>What qualification will I receive?</b><p>You will receive a Level 7 Certificate in Coaching and Mentoring.</p><p>The ILM level 7 Certificate is the equivalent of a postgraduate degree<p>",
                });
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    Name = "ILM 5 Certificate in Coaching and Mentoring",
                    ProviderName = "UK College of Personal Development",
                    Price = 1995,
                    Desription = "<b>What is it?</b><p>The ILM Level 5 in Effective Coaching and Mentoring is professional qualification equivalent to a university foundation degree. You will:</p><ul><li>Learn how to manage the coaching and mentoring process within an organisation</li><li>Deepen your understanding of how the workplace can affect coaching and mentoring</li><li>Plan your future development in coaching and mentoring.</li></ul><b>Who’s it for?</b><p>This course is ideal for those with responsibility for coaching and mentoring others in the workplace such as a manager or team leader. It is also perfect for people interested in embarking upon a career as a professional coach.</p><b>How long will it take?</b><p>On average it will take 10 – 12 months.</p><p><b>How is it assessed?</b></p><p>Assessed by approved ILM Supervisors/markers</p><p>Generally you will work with your provider to get the written and practical assessment completed to ILM standards – depending on which level you choose will determine the amount of assessments – some can just be written essays and some can include supervised coaching sessions. Once your provider is happy with your standard of work it will be sent to the ILM to assess in house.</p><b>What qualification will I receive?</b><p>You will receive a Level 5 Certificate in Coaching and Mentoring (QCF).</p><p>The ILM level 5 is the equivalent of a foundation degree.</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = coachingCategory.Id,
                    Name = "ILM 5 Certificate in Coaching and Mentoring",
                    ProviderName = "Academy of Leadership & Management",
                    Price = 2100,
                    Desription = "<b>What is it?</b><p>The ILM Level 5 in Effective Coaching and Mentoring is professional qualification equivalent to a university foundation degree. You will:</p><ul><li>Learn how to manage the coaching and mentoring process within an organisation</li><li>Deepen your understanding of how the workplace can affect coaching and mentoring</li><li>Plan your future development in coaching and mentoring.</li></ul><b>Who’s it for?</b><p>This course is ideal for those with responsibility for coaching and mentoring others in the workplace such as a manager or team leader. It is also perfect for people interested in embarking upon a career as a professional coach.</p><b>How long will it take?</b><p>On average it will take 10 – 12 months.</p><b>How is it assessed?</b><p>Assessed by approved ILM Supervisors/markers</p><p>Generally you will work with your provider to get the written and practical assessment completed to ILM standards – depending on which level you choose will determine the amount of assessments – some can just be written essays and some can include supervised coaching sessions. Once your provider is happy with your standard of work it will be sent to the ILM to assess in house.</p><b>What qualification will I receive?</b><p>You will receive a Level 5 Certificate in Coaching and Mentoring (QCF).</p><p>The ILM level 5 is the equivalent of a foundation degree.</p>",
                });

                // leadership
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM Level 7 Certificate in Leadership & Management",
                    CourseLevels = new List<CourseLevel>() { new CourseLevel { Level = directorLevel } },
                    Price = 0,
                    Desription = @"<div>
                                      <b class='d-block mb-2'>What is it?</b>
                                      <p>
                                        The ILM Level 7 Certificate In Leadership & Management is one of the most senior qualifications.
                                      </p>
                                      <p>
                                        The course is focused on a combination of executive coaching and work-based learning where you will define a set of objectives designed to have a real impact on your workplace and develop these into a work-based learning project.
                                      </p>
                                      <p>
                                        You will be looking to make the best use of resources, understand the need to innovate and optimise performance within the strategic context of Adapt IQ. You may also have to present arguments for change, construct business cases, lead change implementation and evaluate the impact of that change.
                                      </p>
                                      <p class='mb-4'>
                                        You will see growth in your professional capabilities and personal brand. You will gain valuable experience in working on and improving strategic issues within an Adapt IQ and will grow and develop in ways that are both predictable and not immediately obvious at the start of the course.
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>Who’s it for?</b>
                                      <p class='mb-4'>
                                        This course is designed for aspiring senior managers and leaders who are seeking to develop their professional capability and personal brand, at the same time recognising that they must satisfy various stakeholders.
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How long will it take?</b>
                                      <p class='mb-4'>This is a 4-month blended leading programme that includes executive coaching.</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How is it assessed?</b>
                                      <p class='mb-4'>A journal or portfolio of work will be presented and assessed by the course assessors.</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>What qualification will I receive?</b>
                                      <p class='mb-4'>You will receive an ILM Level 7 Certificate in Leadership & Management which is the equivalent of a master’s degree.</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How much will it cost?</b>
                                      <p>
                                        £1,815 – various payment plans are available.
                                      </p>
                                    </div>"
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM 7 Diploma in Leadership & Management",
                    CourseLevels = new List<CourseLevel>() { new CourseLevel { Level = directorLevel } },
                    Price = 0,
                    Desription = @"<div>
                                      <b class='d-block mb-2'>What is it?</b>
                                      <p class='mb-4'>
                                        A Senior Leadership programme.
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>Who’s it for?</b>
                                      <ul class='mb-4'>
                                        <li>A senior leader and manager who has the ability to think and act strategically</li>
                                        <li>A senior leader and manager who can make informed, evidence based decisions</li>
                                        <li>Motivated staff who can create and maintain a high performance culture</li>
                                        <li>Senior team members who are self-aware and take responsibility for self-development</li>
                                      </ul>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How long will it take?</b>
                                      <p class='mb-4'>12 months</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How is it assessed?</b>
                                      <p class='mb-4'>Journal and portfolio</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>What qualification will I receive?</b>
                                      <p class='mb-4'>ILM 7 Diploma in Leadership & Management</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How much will it cost?</b>
                                      <p>
                                        £3,025
                                      </p>
                                    </div>"
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM Level 5 Certificate in Leadership & Management",
                    CourseLevels = new List<CourseLevel>() { new CourseLevel { Level = midManagerLevel },
                        new CourseLevel { Level = seniorMangerLevel },
                    },
                    Price = 0,
                    Desription = @"<div>
                                      <b class='d-block mb-2'>What is it?</b>
                                      <p>
                                        The ILM Level 5 Certificate In Leadership & Management is the perfect course for aspiring Mid-level Managers. During the course, you’ll learn a raft of new skills including how to use core management techniques to drive better results, develop your ability to lead, motivate and inspire and provide strategic leadership as well as day-to-day management.
                                      </p>
                                      <p class='mb-4'>
                                        Upon completion, you’ll be able to benchmark your managerial skills, develop your capabilities in a way that is tailored to your needs and raise your profile in your organisation.
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>Who’s it for?</b>
                                      <p class='mb-4'>
                                        This course has been designed for middle managers with practical experience. It helps to develop your skills and experience, improve performance and prepare for senior management responsibilities.
                                      </p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How long will it take?</b>
                                      <p class='mb-4'>It will take between 6 - 12 months.</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How is it assessed?</b>
                                      <p class='mb-4'>All assessments are work-related to help provide individual development.</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>What qualification will I receive?</b>
                                      <p class='mb-4'>You will receive an ILM Level 5 Certificate in Leadership & Management which is the equivalent of a foundation degree.</p>
                                    </div>
                                    <div>
                                      <b class='d-block mb-2'>How much will it cost?</b>
                                      <p>
                                        £1,815 - various payment plans are available.
                                      </p>
                                    </div>"
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM Level 3 Certificate in Leadership & Management",
                    CourseLevels = new List<CourseLevel>() { 
                        new CourseLevel { Level = supervisorLevel },
                        new CourseLevel { Level = entryLevel },
                    },
                    Price = 0,
                    Desription = @"<div>
                                    <b class='d-block mb-2'>What is it?</b>
                                    <p>
                                    The ILM Level 3 Certificate In Leadership & Management is the perfect course for aspiring team leaders. It helps nurture strategic thinking at this level of management to foster business improvement and engage middle managers with training and development.
                                    </p>
                                    <p class='mb-4'>This qualification is designed to provide clear, measurable benefits to career-minded professionals. You’ll gain a range of key management skills and put them into practice in your own role. You’ll build your leadership capabilities, motivate and engage teams and manage relationships confidently.
                                    </p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>Who’s it for?</b>
                                    <p>
                                    The ILM Level 3 Certificate in Leadership & Management programme has been designed for individuals who have management responsibilities but no formal training, and are serious about developing their abilities.
                                    </p>
                                    <p class='mb-4'>
                                    It particularly supports team leaders seeking to move up to the next level of management and managers who need to lead people in large or small teams. This training and qualification provide a solid foundation for their role as leader/manager in a business or organisation.
                                    </p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How long will it take?</b>
                                    <p class='mb-4'>It will take between 6 - 12 months.</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How is it assessed?</b>
                                    <p class='mb-4'>A workbook or portfolio will be assessed by the course assessor.</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>What qualification will I receive?</b>
                                    <p class='mb-4'>You will receive an ILM Level 3 Certificate in Leadership & Management.</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How much will it cost?</b>
                                    <p>£1,494 - various payment plans are available.</p>
                                </div>"
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM 3 Award in Leadership & Management",
                    CourseLevels = new List<CourseLevel>() {
                        new CourseLevel { Level = supervisorLevel },
                        new CourseLevel { Level = entryLevel },
                    },
                    Price = 0,
                    Desription = @"<div>
                                    <b class='d-block mb-2'>What is it?</b>
                                    <p class='mb-4'>Management Training Programme</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>Who’s it for?</b>
                                    <p class='mb-4'>
                                    The ILM Level 3 Award in Leadership & Management qualification has been designed for individuals who have management responsibilities but no formal training, and are serious about developing their abilities.
                                    </p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How long will it take?</b>
                                    <p class='mb-4'>2-4 months</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How is it assessed?</b>
                                    <p class='mb-4'>2 assignments</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>What qualification will I receive?</b>
                                    <p class='mb-4'>You will receive an ILM Level 3 Award in Leadership & Management.</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How much will it cost?</b>
                                    <p>£695 - various payment plans are available.</p>
                                </div>"
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM Level 2 certificate in Leadership & Management",
                    CourseLevels = new List<CourseLevel>() {
                        new CourseLevel { Level = supervisorLevel },
                        new CourseLevel { Level = entryLevel },
                    },
                    Price = 0,
                    Desription = @"<div>
                                    <b class='d-block mb-2'>What is it?</b>
                                    <p>
                                    The ILM Level 2 Certificate in Team Leading qualification course for team leaders is a practical, highly interactive course complete with a comprehensive qualification workbook and a training manual.
                                    </p>
                                    <p class='mb-4'>
                                    The course will help develop you as a team leader, plan and monitor your workload, improve team performance through the development of your communication skills. You’ll learn how to set objectives, implement quality systems and manage workplace change and innovation.
                                    </p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>Who’s it for?</b>
                                    <p class='mb-4'>
                                    The ILM Level 2 Certificate is a fantastic foundation course for new or aspiring team leaders, designed to give them the skills and knowledge, and confidence for a smooth transition from team member to team leader.
                                    </p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How long will it take?</b>
                                    <p class='mb-4'>It will take between 4-6 months.</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How is it assessed?</b>
                                    <p class='mb-4'>A workbook or portfolio will be assessed by the course assessor.</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>What qualification will I receive?</b>
                                    <p class='mb-4'>You will receive an ILM Level 2 Certificate in Leadership & Management.</p>
                                </div>
                                <div>
                                    <b class='d-block mb-2'>How much will it cost?</b>
                                    <p>£996 - monthly payment plans are available.</p>
                                </div>"
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM 7 Certificate in Leadership & Management",
                    ProviderName = "Academy of Leadership & Management",
                    Price = 1980,
                    Desription = "<b>What is it?</b><p>The ILM Level 7 Certificate In Leadership & Management is one of the most senior qualifications.</p><p>The course is focused on a combination of executive coaching and work-based learning where you will define a set of objectives designed to have a real impact on your workplace and develop these into a work-based learning project.</p><p>You will be looking to make the best use of resources, understand the need to innovate and optimise performance within the strategic context of Adapt IQ. You may also have to present arguments for change, construct business cases, lead change implementation and evaluate the impact of that change.</p><p>You will see growth in your professional capabilities and personal brand. You will gain valuable experience in working on and improving strategic issues within an Adapt IQ and will grow and develop in ways that are both predictable and not immediately obvious at the start of the course.</p><b>Who’s it for?</b><p>This course is designed for aspiring senior managers and leaders who are seeking to develop their professional capability and personal brand, at the same time recognising that they must satisfy various stakeholders.</p><b>How long will it take?</b><p>This is a 4-month blended leading programme that includes executive coaching.</p><b>How is it assessed?</b><p>A journal or portfolio of work will be presented and assessed by the course assessors.</p><b>What qualification will I receive?</b><p>You will receive an ILM Level 7 Certificate in Leadership & Management which is the equivalent of a master’s degree.</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM 7 Diploma in Leadership & Management",
                    ProviderName = "Academy of Leadership & Management",
                    Price = 3300,
                    Desription = "<b>What is it?</b><p>A Senior Leadership programme.</p><b>Who’s it for?</b><ul><li>A senior leader and manager who has the ability to think and act strategically</li><li>A senior leader and manager who can make informed, evidence based decisions</li><li>Motivated staff who can create and maintain a high performance culture</li><li>Senior team members who are self-aware and take responsibility for self-development</li></ul><b>How long will it take?</b><p>12 months</p><b>How is it assessed?</b><p>Journal and portfolio</p><b>What qualification will I receive?</b><p>ILM 7 Diploma in Leadership & Management</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM 5 Certificate in Leadership & Management",
                    ProviderName = "Academy of Leadership & Management",
                    Price = 1800,
                    Desription = "<b>What is it?</b><p>The ILM Level 5 Certificate In Leadership & Management is the perfect course for aspiring Mid-level Managers. During the course, you’ll learn a raft of new skills including how to use core management techniques to drive better results, develop your ability to lead, motivate and inspire and provide strategic leadership as well as day-to-day management.</p><p>Upon completion, you’ll be able to benchmark your managerial skills, develop your capabilities in a way that is tailored to your needs and raise your profile in your organisation.</p><b>Who’s it for?</b><p>This course has been designed for middle managers with practical experience. It helps to develop your skills and experience, improve performance and prepare for senior management responsibilities.</p><b>How long will it take?</b><p>It will take between 6 - 12 months.</p><b>How is it assessed?</b><p>All assessments are work-related to help provide individual development.</p><b>What qualification will I receive?</b><p>You will receive an ILM Level 5 Certificate in Leadership & Management which is the equivalent of a foundation degree.</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM 3 Certificate in Leadership & Management",
                    ProviderName = "UK College of Personal Development",
                    Price = 1495,
                    Desription = "<b>What is it?</b><p>The ILM Level 3 Certificate In Leadership & Management is the perfect course for aspiring team leaders. It helps nurture strategic thinking at this level of management to foster business improvement and engage middle managers with training and development.</p><p>This qualification is designed to provide clear, measurable benefits to career-minded professionals. You’ll gain a range of key management skills and put them into practice in your own role. You’ll build your leadership capabilities, motivate and engage teams and manage relationships confidently.</p><b>Who’s it for?</b><p>The ILM Level 3 Certificate in Leadership & Management programme has been designed for individuals who have management responsibilities but no formal training, and are serious about developing their abilities.</p><p>It particularly supports team leaders seeking to move up to the next level of management and managers who need to lead people in large or small teams. This training and qualification provide a solid foundation for their role as leader/manager in a business or organisation.</p><b>How long will it take?</b><p>It will take between 6 - 12 months.</p><b>How is it assessed?</b><p>A workbook or portfolio will be assessed by the course assessor.</p><b>What qualification will I receive?</b><p>You will receive an ILM Level 3 Certificate in Leadership & Management.</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM 3 Certificate in Leadership & Management",
                    ProviderName = "Academy of Leadership & Management",
                    Price = 1650,
                    Desription = "<b>What is it?</b><p>The ILM Level 3 Certificate In Leadership & Management is the perfect course for aspiring team leaders. It helps nurture strategic thinking at this level of management to foster business improvement and engage middle managers with training and development.</p><p>This qualification is designed to provide clear, measurable benefits to career-minded professionals. You’ll gain a range of key management skills and put them into practice in your own role. You’ll build your leadership capabilities, motivate and engage teams and manage relationships confidently.</p><b>Who’s it for?</b><p>The ILM Level 3 Certificate in Leadership & Management programme has been designed for individuals who have management responsibilities but no formal training, and are serious about developing their abilities.</p><p>It particularly supports team leaders seeking to move up to the next level of management and managers who need to lead people in large or small teams. This training and qualification provide a solid foundation for their role as leader/manager in a business or organisation.</p><b>How long will it take?</b><p>It will take between 6 - 12 months.</p><b>How is it assessed?</b><p>A workbook or portfolio will be assessed by the course assessor.</p><b>What qualification will I receive?</b><p>You will receive an ILM Level 3 Certificate in Leadership & Management.</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM 3 Award in Leadership & Management",
                    ProviderName = "UK College of Personal Development",
                    Price = 695,
                    Desription = "<b>What is it?</b><p>Management Training Programme</p><b>Who’s it for?</b><p>The ILM Level 3 Award in Leadership & Management qualification has been designed for individuals who have management responsibilities but no formal training, and are serious about developing their abilities.</p><b>How long will it take?</b><p>2-4 months</p><b>How is it assessed?</b><p>2 assignments</p><b>What qualification will I receive?</b><p>You will receive an ILM Level 3 Award in Leadership & Management.</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM 3 Award in Leadership & Management",
                    ProviderName = "Academy of Leadership & Management",
                    Price = 720,
                    Desription = "<b>What is it?</b><p>Management Training Programme</p><b>Who’s it for?</b><p>The ILM Level 3 Award in Leadership & Management qualification has been designed for individuals who have management responsibilities but no formal training, and are serious about developing their abilities.</p><b>How long will it take?</b><p>2-4 months</p><b>How is it assessed?</b><p>2 assignments</p><b>What qualification will I receive?</b><p>You will receive an ILM Level 3 Award in Leadership & Management.</p>",
                });
                courses.Add(new Course
                {
                    CategoryId = leadershipCategory.Id,
                    Name = "ILM 2 Certificate in Leadership & Management",
                    ProviderName = "UK College of Personal Development",
                    Price = 995,
                    Desription = "<b>What is it?</b><p>The ILM Level 2 Certificate in Team Leading qualification course for team leaders is a practical, highly interactive course complete with a comprehensive qualification workbook and a training manual.</p><p>The course will help develop you as a team leader, plan and monitor your workload, improve team performance through the development of your communication skills. You’ll learn how to set objectives, implement quality systems and manage workplace change and innovation.</p><b>Who’s it for?</b><p>The ILM Level 2 Certificate is a fantastic foundation course for new or aspiring team leaders, designed to give them the skills and knowledge, and confidence for a smooth transition from team member to team leader.</p><b>How long will it take?</b><p>It will take between 4-6 months.</p><b>How is it assessed?</b><p>A workbook or portfolio will be assessed by the course assessor.</p><b>What qualification will I receive?</b><p>You will receive an ILM Level 2 Certificate in Leadership & Management.</p>",
                });
                // ned
                courses.Add(new Course
                {
                    CategoryId = nedCategory.Id,
                    Name = "So you want to be a NED workshop",
                    Price = 450,
                    Desription = "<div><b>Who Attends?</b><ul><li>Individuals who are looking to secure their first non-executive director role</li><li>Individuals who have been approached about a role and want to gain a better understanding of what this will entail</li></ul><b>Key Benefits</b><ul><li>Get a better understanding of the role of a NED, including the key differences between an executive and a non-executive director</li><li>Become more familiar with the softer skills needed to be an effective NED</li><li>Understand how boards are structured by joining discussions with leading experts and experienced non-executive directors</li><li>Network with like-minded professionals and find out how you can secure that all-important first role</li><li>Grow your network, collaborate, and be part of an increasing, dynamic and diverse Alumni Board Member community</li><li>Receive exclusive access to our regular networking evenings including speaking sessions with recognised thought-leaders upon completing the course</li><li>Upon completing the course, delegates will receive exclusive access to our regular networking evenings including speaking sessions with recognised thought leaders.</li></ul><b>Programme Content</b><ul><li>Effective behavior in the boardroom</li><li>Legal liabilities and corporate governance</li><li>Board structure and dynamics</li><li>Life as a non-executive: a personal view from an experienced NED</li><li>The opportunities and risks of being a NED</li><li>How to build a portfolio career, present your CV and interview techniques</li></ul><b>How much will it cost?</b><p>£450</p></div>",
                    ProviderName = "Financial Times",
                    ProviderPrice = 450
                });
                courses.Add(new Course
                {
                    CategoryId = nedCategory.Id,
                    Name = "The Effective Non-Executive Director Programme Workshop",
                    Price = 3500,
                    Desription = "<div><b>Who Attends?</b><ul><li>Existing non-executive directors who wish to further develop their skills</li><li>Professionals who have just taken on their first non-executive position and are keen to prepare themselves for a non-executive career</li><li>Individuals looking to develop themselves for future board positions</li></ul><b>Why Choose The Effective Non-Executive Director Programme?</b><ul><li>Get uniquely practical and relevant insights on how to build a successful NED career from a practioner-led faculty</li><li>Receive exclusive access to our regular networking evenings including speaking sessions with recognised thought-leaders upon completing the Course</li><li>Build greater confidence to prepare for the challenges ahead in your portfolio career</li><li>Improve your board effectiveness by exploring the importance of strategic thinking on boards and how you can add value to the organisation</li><li>Get a deeper understanding of the legal, regulatory, and governance issues facing board members today</li><li>Understand the importance of board behaviour in strengthening company performance</li><li>Expand your knowledge of sub-committees and how they can work effectively</li><li>Become more aware of the importance of strategic thinking on boards and how you can add value to the organisation</li><li>Find out how to successfully develop your personal NED career with leading experts and experienced non-executive directors</li><li>Network with like-minded professionals</li><li>Develop the most important soft skills required to be a NED during and post-pandemic</li><li>Connect and engage with faculty and peers; discuss your pressing questions and debate the issues that really matter for NEDs today</li><li>Grow your network of board members: be part of a growing, dynamic and diverse Alumni NED & Executive Board Member community - your valuable, durable network and catalyst for powerful collaboration</li><li>Upon completing the course, delegates will receive exclusive access to our regular networking evenings including speaking sessions with recognised thought leaders.</li></ul><b>How much will it cost?</b><p>£3,500</p></div>",
                    ProviderName = "Financial Times",
                    ProviderPrice = 3500
                });
                courses.Add(new Course
                {
                    CategoryId = nedCategory.Id,
                    Name = "The Financial Times NED Diploma ",
                    Price = 6900,
                    Desription = "<div><b>Learning Environment</b><p>The 5-module diploma is delivered online to ensure flexibility in your studies. There are two compulsory workshops as part of the course where participants will have the opportunity to learn from and interact with a panel of experts in their respective fields. We will offer the choice to take part in these workshops completely online through our virtual cohorts, or in-person with our blended cohorts. The first half-day workshop at the start of the course will provide an introduction to the topics covered, whilst the second workshop will span over two days, focusing on the all-important issue of effective boardroom behavior. This workshop provides the perfect context for participants to network with each other and develop their skills on an individual basis as well as within a group.</p><b>The modules</b><ol><li>The Effective Non-Executive Director</li><li>Director’s Duties and Liabilities</li><li>Board Structure and Performance</li><li>Audit and Financial Reporting</li><li>Risk Management and Internal Control</li></ol><b>Assessment methods</b><p>The course modules are assessed separately. Module 1 consists of a learning log submission as well as a boardroom prep and boardroom dilemma. Modules 2, 3 and 4 are assessed by a case study submission, and module 5 is assessed by a 3-hour controlled-assessment based on a set of accounts, taken at the end of the course. Each method will make up a third of the candidate's final pass/fail grade.</p><b>Key Benefits</b><ul><li>Improve your board performance by dealing with real-life issues that may occur during your tenure</li><li>Develop the skills necessary to challenge executives and contribute to an effective board culture</li><li>Gain confidence, broaden your knowledge of the role of the board and challenge your own way of thinking</li><li>Increase your individual marketability, helping you get onto stronger boards</li><li>Develop an international network of outstanding alumni</li><li>Improve your understanding of personal strengths and weaknesses in relation to board dynamics</li></ul><b>Why choose the Financial Times Non-Executive Director Diploma?</b><ul><li>Gain a competitive edge by acquiring a fully accredited level 7 post graduate qualification to help you secure new roles</li><li>Conveniently access course content, get ongoing support from your tutors and engage with your peers via an online platform</li><li>Grow your network: be part of an increasing, dynamic and diverse Alumni Board Member community - your valuable, durable network and a catalyst for powerful collaboration</li><li>Be fast-tracked onto the KPMG connect on Board programme connecting non-executives from a diverse talent pool, with organisations seeking to build better boards</li><li>Upon graduation, in addition to your official certification, receive a digital credential to add to your Linkedin profile and have your name printed within the Financial Times newspaper</li><li>The FT Non-Executive Director Diploma cost includes assessment fees, attendance at the workshops, networking sessions, and access to online learning materials and support.</li><li>Delegates will have access to Premium FT.com content for the duration of the course. They will also receive exclusive access to our regular networking and speaking events with recognised thought leaders.</li></ul><b>How much will it cost?</b><p>Virtual: £6,900</p></div>",
                    ProviderName = "Financial Times",
                    ProviderPrice = 7500
                });
                await context.Courses.AddRangeAsync(courses);
                foreach (var course in courses)
                {
                    var url = (string.IsNullOrEmpty(course.ProviderName)) ? course.Name : course.Name + "-by-" + course.ProviderName;
                    var currentCourse = context.Entry(course);
                    currentCourse.Property(e => e.Url).CurrentValue = Common.Utilities.StringHelper.UrlFriendly(url);
                    currentCourse.Property(e => e.IsVisible).CurrentValue = true;
                }
                #endregion
                await context.SaveChangesAsync();
            }
            if (!context.Users.Any())
            {
                #region admin
                var user = new CoursewiseUser
                {
                    Email = "admin@getcoursewise.com",
                    EmailConfirmed = true,
                    UserName = "admin@getcoursewise.com",
                    Name = "Coursewise Admin"
                };
                IdentityResult result = await userManager.CreateAsync(user, "123456");
                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, CoursewiseRoles.ADMIN));
                }
                await context.SaveChangesAsync();
                #endregion
            }
            if (!context.ServiceStatuses.Any())
            {
                var notificationServiceExists = context.ServiceStatuses.FirstOrDefault(a => a.Title == "Notifications");
                if (notificationServiceExists == null)
                {
                    ServiceStatus notificationService = new ServiceStatus();
                    notificationService.Title = "Notifications";
                    await context.ServiceStatuses.AddAsync(notificationService);
                }

            }
            else
            {
                var notificationService = context.ServiceStatuses.FirstOrDefault(a => a.Title == "Notifications");
                if (notificationService != null)
                {
                    notificationService.IsRunning = false;
                    context.ServiceStatuses.Update(notificationService);
                }
            }
            await SeedEmailTemplates(context, configuration);
        }

        public static async Task SeedEmailTemplates(CoursewiseDbContext context, IConfiguration configuration)
        {
            var templates = configuration.GetSection("emailTemplates").Get<List<EmailTemplate>>();
            EmailTemplate? ExistingTemplate = context.EmailTemplates.FirstOrDefault();
            if (ExistingTemplate == null)
            {
                foreach (var temp in templates)
                {
                    context.EmailTemplates.Add(new EmailTemplate() { Body = $"{temp.Body}", Type = temp.Type });
                }
            }
            else
            {
                var existingTemplates = context.EmailTemplates.ToList();
                foreach (var template in templates)
                {
                    var existingTemplate = existingTemplates.Find(t => t.Type == template.Type);
                    if (existingTemplate != null && existingTemplate.Body != template.Body)
                    {
                        existingTemplate.Body = template.Body;
                        context.EmailTemplates.Update(existingTemplate);
                    }
                    else
                    {
                        if (existingTemplate == null)
                        {
                            context.EmailTemplates.Add(new EmailTemplate()
                            {
                                Body = template.Body,
                                Type = template.Type
                            });
                            await context.SaveChangesAsync();
                        }
                    }

                }
            }
            await context.SaveChangesAsync();
        }
    }
}
