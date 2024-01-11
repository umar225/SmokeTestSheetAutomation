import React from 'react';
import { Navbar, Container, Nav } from 'react-bootstrap';
import { App_logo } from '../../utils/image';
import { HiShoppingCart } from 'react-icons/hi';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../routes/useAuth';

function Header() {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const checkAdmin = localStorage.getItem('user');

  const onLogOutPress = () => {
    localStorage.removeItem('access_token');
    logout();
    navigate('/login');
  };

  return (
    <Navbar collapseOnSelect expand="lg" className="app-header" variant="light">
      <Container fluid className="headerWrapperInner">
        <Navbar.Brand href="/">
          <img src={App_logo} alt="app-logo" />
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
          <Nav className="me-auto"></Nav>
          <Nav>
            <Nav.Link
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
            </Nav.Link>
            {checkAdmin && checkAdmin !== 'null' ? (
              <Nav.Link
                className="nav-style headerTextStyle"
                onClick={() => onLogOutPress()}
              >
                Log Out
              </Nav.Link>
            ) : (
              ''
              // <Nav.Link className="nav-style headerTextStyle" href="/login">
              //   Login
              // </Nav.Link>
            )}
            <Nav.Link style={{ color: 'black' }} href="/viewBasket">
              <HiShoppingCart />
            </Nav.Link>

            {/* <NavDropdown title="Dropdown" id="collasible-nav-dropdown">
              <NavDropdown.Item href="#action/3.1">Action</NavDropdown.Item>
              <NavDropdown.Item href="#action/3.2">
                Another action
              </NavDropdown.Item>
              <NavDropdown.Item href="#action/3.3">Something</NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item href="#action/3.4">
                Separated link
              </NavDropdown.Item>
            </NavDropdown> */}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default Header;
