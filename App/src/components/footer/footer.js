import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import env from '../../utils/env.json';

function Footer() {
  return (
<>
    <Container fluid className='d-none d-sm-block footerMain'>
          <Row >
            <Col className='col-6 col-sm-7 '>
              <Row>
                <Col className='col-md-3 col-sm-4'><b>Coursewise </b>©2021</Col>
                <Col className='col-md-3 col-sm-4'><b>
                    <a
                      href="https://getcoursewise.com/terms-and-conditions/"
                      target="_blank"
                    >
                      Terms & Conditions
                    </a>
                  </b></Col>
                <Col className='col-md-3 col-sm-4'><b>
                    <a
                      href="https://getcoursewise.com/privacy-policy/"
                      target="_blank"
                    >
                      Privacy Policy
                    </a>
                  </b></Col>
              </Row>
            </Col>
            <Col className='col-4 col-sm-3 '></Col>
            
            <Col className='col-2 col-sm-2 ' style={{textAlign:'right'}}>Version: 1.{env.AppVersion}</Col>
          </Row> 
      </Container>
      <Container fluid className='d-sm-none footerMobile'>
        <Row className='text-center'>
        <p><b>Coursewise </b>©2021</p>
        </Row>
        <Row className='text-center'>
                  <p><b>
                    <a
                      href="https://getcoursewise.com/terms-and-conditions/"
                      target="_blank"
                    >
                      Terms & Conditions
                    </a>
                  </b></p>
        </Row>
        <Row className='text-center'>
                  <p>
                  <b>
                    <a
                      href="https://getcoursewise.com/privacy-policy/"
                      target="_blank"
                    >
                      Privacy Policy
                    </a>
                  </b>
                  </p>
        </Row>
        <br></br>
        <Row className='text-center'>
        <p>Version: 1.{env.AppVersion}</p>
        </Row>
      </Container>
    </>
  );
}
export default Footer;
