import React, {useEffect, useState} from 'react';
import { AdminHeader } from '../../components/header/index';
import { Container, Row, Col, Form } from 'react-bootstrap';
import Footer from '../../components/footer/footer';
import Pagination from '@mui/material/Pagination';
import { adminCourseActions } from '../../store/adminCourses/adminCourseActions';
import Loader from '../../components/Loader';
import { toast } from 'react-toastify';
import { useDispatch } from 'react-redux';
import moment from 'moment';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';

export default function AdminDashboard() {
    const dispatch = useDispatch();

    const [isLoading, setIsLoading] = useState(false);
    const [customers, setCustomers] = useState([]);
    const [totalPages, setTotalPages] = useState(1);
    const [page, setPage] = useState(1);
    const [searchEmail, setSearchEmail] = useState('');
    const [userType, setUserType] = useState('all');
    const [userId, setUserId] = useState('');
    const [isOneToOneMember, setIsOneToOneMember] = useState(false);
    const [showModal, setShowModal] = useState(false);

    const categorySort = (
        <div className="sortMainWrapper">
            <div className="sortRow">
                <div className="jobCategoryContainer adminCategory">
                  <input
                    name="external-exclusive"
                    type="radio"
                    autoComplete="off"
                    id="radio-all"
                    value="all"
                    checked={userType === 'all'}
                    onClick={(event)=>setUserType(event.target.value)}
                  />
                  <label htmlFor="radio-all">
                    All
                  </label>
                  <input
                    name="external-exclusive"
                    type="radio"
                    autoComplete="off"
                    id="radio-exclusive"
                    value="subscribed"
                    checked={userType === 'subscribed'}
                    onClick={(event)=>setUserType(event.target.value)}
                  />
                  <label htmlFor="radio-exclusive">
                  Subscribed
                  </label>
                  <input
                    name="external-exclusive"
                    type="radio"
                    autoComplete="off"
                    id="radio-external"
                    value="unsubscribed"
                    checked={userType === 'unsubscribed'}
                    onClick={(event)=>setUserType(event.target.value)}
                  />
                  <label htmlFor="radio-external">
                    Unsubscribed
                  </label>
                </div>
            </div>
        </div>
    )

    const getCustomers = (event, value) => {
        setIsLoading(true);
        setPage(value);
        let currentPage = value || page;
        dispatch(adminCourseActions.searchCustomers({email: searchEmail, userType: userType, page: currentPage})).then((res) => {
            setIsLoading(false);
            if (res?.success) {
              setCustomers(res.data.users);
              setTotalPages(res.data.pagination.totalPages);
              setPage(res.data.pagination.currentPage);
            } else {
              toast.error(res?.message);
            }
        });
    }
    const subscribeMember = (id, subscribe) => {
        setIsLoading(true);
        dispatch(adminCourseActions.subscribeOneToOne({id: id, subscribe: subscribe})).then((res) => {
            setIsLoading(false);
            setShowModal(false);
            if (res?.success) {
                toast.success(res?.message);
                getCustomers();
            } else {
                toast.error(res?.message);
            }
        })
    }
    const search = (e) => {
        if (e.code== "Enter") 
        getCustomers()
    }

    useEffect(() => {
        setIsLoading(true);
        getCustomers();
      }, [userType]);

  return (
    <div>
      <AdminHeader />
      <div className="bodyWrapper">
        <div className="mainWrapper">
          <div className="whiteBackground">
            <Container className="projectDetail">
                {isLoading && <Loader />}
                <div style={{display:"flex", gap:"20px"}}>
                    <div className='searchBar'>
                        <input value={searchEmail}
                        onChange={(e) => setSearchEmail(e.target.value)} 
                        placeholder="Search Email"
                        onKeyDown={search} />
                    </div>
                    <div className='categorySortMain'>
                        {categorySort}
                    </div>
                </div>
                <table style={{ width: '100%' }} className="cartItemsTable">
                    <tbody>
                        <tr>
                          <th style={{ width: '25%' }}>Email</th>
                          <th style={{ width: '25%' }}>Name</th>
                          <th style={{ width: '25%' }}>1-2-1 Member</th>
                          <th style={{ width: '25%' }}>Expiry</th>
                        </tr>
                        {customers.map((row) => (
                        <tr key={row.id}>
                          <td>{row.email}</td>
                          <td>{row.name}</td>
                          <td>
                            <Form>
                                <div className="showHideWrapper">
                                    <Form.Check
                                    type="switch"
                                    id="custom-switch"
                                    checked={row.isOneToOneMember}
                                    onChange={() =>{
                                        setUserId(row.id); 
                                        setIsOneToOneMember(!row.isOneToOneMember); 
                                        setShowModal(true)
                                    }} />
                                </div>
                            </Form>
                          </td>     
                        <td>
                            {row.isOneToOneMember && moment(row.oneToOneExpiryDate).utc().format("DD/MM/YYYY")}
                        </td>
                    </tr>
                    ))}
                </tbody>
            </table>
            <Pagination count={totalPages} shape="rounded" page={page} onChange={getCustomers} />
        </Container>
        <Modal size="md" show={showModal} onHide={() => setShowModal(false)}>
            <Modal.Body>
              <Container>
                <div className="courseDetail crsModalWidth">
                  <Row>
                    <Col sm={12}>
                      <p>Are you sure you want to change membership?</p>
                    </Col>
                  </Row>
                </div>
              </Container>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={() => setShowModal(false)}>
                    Cancel
                </Button>
                <Button id="confirmDelete" variant="primary" onClick={()=>subscribeMember(userId, isOneToOneMember)}>
                  Confirm
                </Button>
            </Modal.Footer>
        </Modal>
    </div>
</div></div>
<div className="footerWrapper">
    <Footer />
</div></div>);
}
