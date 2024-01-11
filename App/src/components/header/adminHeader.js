import React from 'react';
import { Navbar, Container, Nav } from 'react-bootstrap';
import { Boardwise_logo_header } from '../../utils/image';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../routes/useAuth';

function AdminHeader() {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const onPressLogOut = () => {
    localStorage.removeItem('access_token');
    logout();
    navigate('/login');
  };
  return (
    <Navbar collapseOnSelect expand="lg" className="app-header" variant="light">
      <Container fluid className="headerWrapperInner">
        <Navbar.Brand href="/admindashboard">
          <img src={Boardwise_logo_header} alt="app-logo" />
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
          <Nav className="me-auto"></Nav>
          <Nav>
            {/* <Nav.Link
              className="nav-style headerTextStyle"
              href="/courselibrary"
            >
              Course library
            </Nav.Link>
            <Nav.Link
              className="nav-style headerTextStyle"
              href="/find-a-course"
            >
              Find a course
            </Nav.Link> */}
            <Nav.Link
              className="nav-style headerTextStyle"
              onClick={() => onPressLogOut()}
            >
              Log Out
            </Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default AdminHeader;
