import { dashboardApi } from './dashboardApi';

const onGetDashboard = () => async () => {
    return dashboardApi.dashboard();
};

export const dashboardActions = {
    onGetDashboard: onGetDashboard,
};
