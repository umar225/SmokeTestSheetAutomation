import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import jobs from '../../utils/images/jobs.svg';
import jobs_active from '../../utils/images/jobs-active.svg';
import resources from '../../utils/images/resources.svg';
import resources_active from '../../utils/images/resources-active.svg';
import pricing from '../../utils/images/pricing.svg';
import pricing_active from '../../utils/images/pricing-active.svg';
import account from '../../utils/images/account.svg';
import account_active from '../../utils/images/account-active.svg';

function MobileNav() {
  const location = useLocation();
  const navigate = useNavigate();

  return (
    <div className="mobileNav">
      <button 
        onClick={() => navigate('/jobs-board')}
      >
      {(location.pathname === '/jobs-board' || location.pathname.startsWith('/job/')) ? (
        <img src={jobs_active} alt="active jobs board" />
        ) : (
          <img src={jobs} alt="jobs board" />
        )}
        <p className={(location.pathname === '/jobs-board' || location.pathname.startsWith('/job/')) && "active" }>Jobs</p>
      </button>
      <button onClick={()=>navigate('/resourcecenter')}>
        {location.pathname === '/resourcecenter' ? (
        <img src={resources_active} alt="active resource center" />
        ) : (
          <img src={resources} alt="resource center" />
        )}
        <p className={location.pathname === '/resourcecenter' && "active" }>Resources</p>
      </button>
      <button onClick={()=>navigate('/pricing')}>
      {location.pathname === '/pricing' ? (
        <img src={pricing_active} alt="active pricing" />
        ) : (
          <img src={pricing} alt="pricing" />
        )}
        <p className={location.pathname === '/pricing' && "active" }>Pricing</p>
      </button>
      <button onClick={()=>navigate('/account')}>
      {location.pathname === '/account' ? (
        <img src={account_active} alt="active account" />
        ) : (
          <img src={account} alt="account" />
        )}
        <p className={location.pathname === '/account' && "active" }>Account</p>
      </button>
    </div>
  );
}

export default MobileNav;
