import React from 'react';
import { AdminHeader } from '../../components/header/index';
import { useNavigate } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import Footer from '../../components/footer/footer';

export default function AdminDashboard() {
  const navigate = useNavigate();

  return (
    <div>
      <AdminHeader />
      <div className="bodyWrapper">
        <div className="mainWrapper">
          <div className="whiteBackground">
            <Container className="projectDetail">
              <Row>
                <Col sm={12}>
                  <h1>Admin Dashboard</h1>
                  <div className="adminDashboardWrapper">
                    <button
                      className="buttonStyleSimple"
                      onClick={() => navigate('/adminjobportal')}
                    >
                      Job portal
                    </button>
                    <button
                      className="buttonStyleSimple"
                      onClick={() => navigate('/adminresourcecenter')}
                    >
                      Resource Center
                    </button>
                    <button
                      className="buttonStyleSimple"
                      onClick={() => navigate('/one2one')}
                    >
                      View All Members
                    </button>
                  </div>
                </Col>
              </Row>
            </Container>
          </div>
        </div>
      </div>
      <div className="footerWrapper">
        <Footer />
      </div>
    </div>
  );
}
