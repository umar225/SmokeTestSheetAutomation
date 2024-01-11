import React from 'react';
import { Navbar, Nav, NavDropdown, Container } from 'react-bootstrap';
import { Boardwise_logo_header } from '../../utils/image';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../routes/useAuth';
import Amplitude from '../../utils/Amplitude';

function UserHeader() {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const displayName = localStorage.getItem('displayName');

  const onPressLogOut = () => {
    localStorage.removeItem('access_token');
    localStorage.removeItem('displayName');
    Amplitude("Logout Success");
    logout();
    navigate('/login');
  };
  
  const navigatetoDashboard=()=>{
    Amplitude('Go to Homepage');
    navigate('/userdashboard');
  }
  const navigateOneToOne=()=>
  {
    Amplitude('Views 1-2-1 Plan');
    window.open('https://www.getboardwise.com/services', '_blank');

  }
  const navigatePrivacyPolicy=()=>
  {
    Amplitude('View Privacy Policy');
    window.open('https://www.getboardwise.com/privacy-policy', '_blank');

  }
  const navigateTandC=()=>
  {
    Amplitude('View T&Cs');
    window.open('https://www.getboardwise.com/terms-conditions', '_blank');

  }
  return (
    <div>
      <Navbar
        collapseOnSelect
        expand="lg"
        className="userHeaderWrapper"
        variant="light"
      >
        <Container fluid className="headerWrapperInner">
          <div className="userNavBar">
          <Navbar.Brand onClick={navigatetoDashboard}>
            <img className='headerLogo' src={Boardwise_logo_header} alt="app-logo" />
          </Navbar.Brand>
          <div className="userNavigation">
              <button onClick={()=>navigate('/jobs-board')}>Jobs</button>
              <button onClick={()=>navigate('/resourcecenter')}>Resource Center</button>
              <button onClick={navigateOneToOne}>1-2-1 Target</button>
              <button onClick={()=>navigate('/pricing')}>Compare Plans</button>
            </div>
          </div>
          
          <div className="userNavBar">
            
            <Nav>
              <div className="avatarDDWraper">
                <NavDropdown
                  title={displayName}
                  className="profileDD"
                  id="collasible-nav-dropdown"
                  onClick={() => Amplitude("View Account Dropdown")}

                >
                  <NavDropdown.Item onClick={()=>navigate('/account')}>
                    Your Account
                  </NavDropdown.Item>
                  <NavDropdown.Item onClick={() => navigateTandC()}>
                    Terms and Conditions
                  </NavDropdown.Item>
                  <NavDropdown.Item  onClick={() => navigatePrivacyPolicy()}>
                    Privacy Policy
                  </NavDropdown.Item>
                  <NavDropdown.Item onClick={() => onPressLogOut()}>
                    Log Out
                  </NavDropdown.Item>
                </NavDropdown>
              </div>
              <span className="customMargin" />

              <span className="customMargin" />

            </Nav>
          </div>
        </Container>
      </Navbar>
    </div>
  );
}

export default UserHeader;
