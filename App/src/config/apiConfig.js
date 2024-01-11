let rootURL = window.env?.URL;

export const endpoints = {
  home: {
    getFormResponse: `${rootURL}feedback/`,
    dashboard: `${rootURL}api/dashboard/get`,
  },
  account: {
    login: `${rootURL}account/login`,
    userLogin: `${rootURL}account/external/login`,
    forgotPassword: `${rootURL}account/forgot-password`,
    resendConfirmation: `${rootURL}account/resend-confirmation`,
    resetPassword: `${rootURL}account/reset-password`,
    registerUser: `${rootURL}account/register`,
    confirmEmail: `${rootURL}account/confirm-email`,
    adminLogin: `${rootURL}account/admin/login`,
    changePassword: `${rootURL}account/change-password`,
    userProfile: `${rootURL}api/customer/basicInfo`,
    updateNotifications: `${rootURL}customer/updateNotifications`,
    linkedInAccessToken:  `${rootURL}account/linkedin/accesstoken`,
    createCheckout: `${rootURL}payment/createCheckout`,
    pricingDetails: `${rootURL}payment/PricingDetails`,
    cancelSubscription: `${rootURL}payment/CancelSubscription`,
  },
  admin: {
    allCategory: `${rootURL}Category/all`,
    getCategoryDetail: `${rootURL}Courses/all/by/category/`,
    getAdminCourseDetail: `${rootURL}Courses/by/id/`,
    updataCourse: `${rootURL}Courses`,
    addResource: `${rootURL}api/resource`,
    getResource: `${rootURL}api/resource/`,
    updateResource: `${rootURL}api/resource`,
    updateResourceMedia: `${rootURL}api/resource/upload/media`,
    deleteResourceMedia: `${rootURL}api/resource/delete/media/`,
    deleteResource: `${rootURL}api/resource/delete/`,

  },
  courses: {
    getPmpCourses: `${rootURL}courses/pmp`,
    getCoachingCourses: `${rootURL}courses/coaching`,
    getLeadershipCourses: `${rootURL}courses/leadership`,
    getCourseDetail: `${rootURL}Courses/library`,
    getNedTraining: `${rootURL}Courses/ned`,
    getCourseLevel: `${rootURL}api/Level/all`,
    addCourse: `${rootURL}Courses`,
    deleteCourse: `${rootURL}Courses/`,
  },
  category: {
    addCategory: `${rootURL}Category`,
    toggleCategory: `${rootURL}Category/toggle`,
    editCategory: `${rootURL}Category`,
  },
  courseLibrary: {
    getAllCourseLibrary: `${rootURL}Courses/library`,
  },
  adminJobs: {
    getAllIndustory: `${rootURL}api/Industry/all`,
    getAllSkills: `${rootURL}api/Skill/all`,
    getAllLocation: `${rootURL}api/Location/all`,
    getAllTitles: `${rootURL}api/Title/all`,
    addAdminJob: `${rootURL}api/job`,
    deleteJob: `${rootURL}api/job/`,
    getAllJobs: `${rootURL}api/Job/all`,
    getJobById: `${rootURL}api/Job/`,
    uploadImg: `${rootURL}api/Job/upload/picture`,
    updateJob: `${rootURL}api/job`,
  },
  adminOneToOne: {
    searchCustomers: `${rootURL}api/Customer/oneToOne`,
    subscribeOneToOne: `${rootURL}api/Customer/subscribeOneToOne`,
  },
  adminResource: {
    getAllResources: `${rootURL}api/resource/all`,
  },
  userJobs: {
    getJobsByFilters: `${rootURL}api/job/filters`,
    getAllFilterValue: `${rootURL}api/Job/get/filters`,
    getUserJobById: `${rootURL}api/job/get/`,
    applyJob: `${rootURL}api/customer/apply/job`,
  },
  resourceCenter: {
    getResources: `${rootURL}api/resource/filters`,
    getResourceByUrl: `${rootURL}api/resource/get/`,
    resourceApplause: `${rootURL}api/resource/applause/`,
    getFeaturedResource: `${rootURL}api/resource/get/featured`,
  },
  baseUrl: rootURL,
};