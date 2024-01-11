import React, { useEffect, useState } from 'react';
import { AdminHeader } from '../../components/header/index';
import { Container, Row, Col } from 'react-bootstrap';
import Footer from '../../components/footer/footer';
import Button from 'react-bootstrap/Button';
import { useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { adminCourseActions } from '../../store/adminCourses/adminCourseActions';
import Loader from '../../components/Loader';
import { toast } from 'react-toastify';

export default function AdminJobPortal() {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const [isLoading, setIsLoading] = useState(false);
  const [allJobs, setAllJobs] = useState([]);

  useEffect(() => {
    setIsLoading(true);
    dispatch(adminCourseActions.onGetAllJobs()).then((res) => {
      setIsLoading(false);
      if (res?.success) {
        setAllJobs(res.data);
      } else {
        toast.error(res?.message);
      }
    });
  }, []);
  return (
    <div>
      <AdminHeader />
      {isLoading && <Loader />}
      <div className="bodyWrapper">
        <div className="mainWrapper">
          <div className="whiteBackground">
            <Container className="projectDetail">
              <Row>
                <Col sm={8}>
                  <div>
                    <div className="addCatWrapper">
                      <Button
                        onClick={() =>
                          navigate('/adminjob', {
                            state: {
                              id: 'new',
                            },
                          })
                        }
                      >
                        Add new Job
                      </Button>
                    </div>
                    <h1>Admin Job portal</h1>
                    <div className="jobWrapper">
                      {allJobs?.length > 0 &&
                        allJobs.map((job) => (
                          <button
                            key={job.id}
                            className="jobBox"
                            onClick={() => {
                              navigate(`/adminjob`, {
                                state: {
                                  id: job.id,
                                },
                              });
                            }}
                          >
                            <div className="jobBoxHeader">
                              <span className="imageContainer">
                                <img src={job.companyLogo} alt="companyLogo" />
                              </span>
                              <span className="jobTitle">{job.company}</span>
                            </div>
                            <div className="jobBoxMid">
                              <span className="jobTxtDescription">
                                {job.previewText}
                              </span>
                            </div>
                            <div className="jobBoxFooter">
                              <span className="jobFtrTxt">
                                {`${job.noOfRole}${' roles'}`}
                              </span>
                            </div>
                          </button>
                        ))}
                    </div>
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
