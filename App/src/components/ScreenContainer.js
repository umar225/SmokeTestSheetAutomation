import React, {useState, useEffect} from 'react';
import { UserHeader } from './header/index';
import Container from 'react-bootstrap/Container';
import PropTypes from 'prop-types';
import MobileNav from './footer/mobileNav';

export const ScreenContainer = ({ children}) => {
  const [width, setWidth] = useState(window.innerWidth);
  const breakpoint = 767;
  useEffect(()=> {
    const handleWindowResize = () => setWidth(window.innerWidth);
    window.addEventListener('resize', handleWindowResize);
  },[]);
  return (
    <div>
      {width >= breakpoint && <UserHeader />}
      <div className="userMargin">
        <Container fluid className="containerWrapper">
          {children}
        </Container>
      </div>
      {width < breakpoint && <MobileNav />}
    </div>
  );
};

ScreenContainer.propTypes = {
  children: PropTypes.node.isRequired,
};
