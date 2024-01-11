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

export default function AdminResourceCenter() {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const options = { year: 'numeric', month: 'long', day: 'numeric' };
  const [isLoading, setIsLoading] = useState(false);
  const [allResources, setAllResources] = useState([]);

  useEffect(() => {
    setIsLoading(true);
    dispatch(adminCourseActions.onGetAllResources()).then((res) => {
      setIsLoading(false);
      if (res?.success) {
        setAllResources(res.data);
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
                <Col>
                  <div>
                    <div className="addCatWrapper">
                      <Button
                        onClick={() =>
                          navigate('/adminresource', {
                            state: {
                              id: 'new',
                            },
                          })
                        }
                      >
                        Add new Resource
                      </Button>
                    </div>
                    <h1>Admin Resource Center</h1>
                    <div className="resourceWrapper">
                      {allResources?.length > 0 &&
                        allResources.map((resource) => (
                          <button
                            key={resource.id}
                            className="resourceBox"
                            onClick={() =>
                              navigate('/adminresource', {
                                state: {
                                  id: resource.id,
                                },
                              })
                            }
                          >
                            <div className="boxHeader">
                              <span className="imageContainer">
                                <img src={resource.media} alt="media" />
                              </span>
                              <span className="readTime">
                                {resource.length} min read
                              </span>
                              <span className="resourceTitle">
                                {resource.title}
                              </span>
                              <span>
                                {resource.preview.slice(0,150)}
                                <b className="jobCardTextColor">
                                  {' '}
                                  ...read more
                                </b>
                              </span>
                            </div>
                            <div className="resourceBoxFooter">
                              <span className="resourceDate">
                                {new Date(
                                  resource.time + 'Z'
                                ).toLocaleDateString('en', options)}
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
