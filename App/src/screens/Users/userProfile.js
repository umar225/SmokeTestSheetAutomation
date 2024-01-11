import React, { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { ScreenContainer } from '../../components/ScreenContainer';
import { Row, Col } from 'react-bootstrap';
import Loader from '../../components/Loader';
import { accountActions } from '../../store/account/accountActions';
import { toast } from 'react-toastify';
import Amplitude from '../../utils/Amplitude';
import Form from 'react-bootstrap/Form';
import TextField from '@mui/material/TextField';
import Radio from '@mui/material/Radio';
import RadioGroup from '@mui/material/RadioGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import { passwordRegx } from '../../utils/validationCheck';
import moment from 'moment';
import { useAuth } from '../../routes/useAuth';
import { useNavigate } from 'react-router-dom';
import GetBoardwiseHeader from '../../utils/images/getBoardwiseHeader.svg';
import { AiFillEyeInvisible, AiFillEye } from 'react-icons/ai';
import InputAdornment from '@mui/material/InputAdornment'; 
import IconButton from '@mui/material/IconButton';
import CancelSubsModal from './cancelSubsModal';

const comparePasswords = (matchPasswords, newPasswordConfirm) => {
  if (!matchPasswords && newPasswordConfirm) {
    return (
    <p className="errorTxt">Passwords don't match</p>
    )
  }
  else {
    return null;
  }
}

function UserProfile() {
  const dispatch = useDispatch();
  const { logout } = useAuth();
  const navigate = useNavigate();

  const [isLoading, setIsLoading] = useState(false);
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [expiryDateValid, setExpiryDateValid] = useState(false);
  const [changePwd, setChangePwd] = useState(false);
  const [notifications, setNotifications] = useState(false);
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [newPasswordConfirm, setNewPasswordConfirm] = useState('');
  const [isValidPassword, setIsValidPassword] = useState(true);
  const [matchPasswords, setMatchPasswords] = useState(true);
  const [isError, setIsError] = useState(false);
  const [jobNotifications, setJobNotifications] = useState(false);
  const [resourceNotifications, setResourceNotifications] = useState(false);
  const [notificationsDisabled, setNotificationsDisabled] = useState(true);
  const [showCurrentPwd, setShowCurrentPwd] = useState(false);
  const [showNewPwd, setShowNewPwd] = useState(false);
  const [showNewPwd2, setShowNewPwd2] = useState(false);
  const [cancelSubsModalShow, setCancelSubsModalShow] = useState(false);
  const [subscriptionChange, setSubscriptionChange] = useState(true);
  const [toggleEditMembership, setToggleEditMembership] = useState(true);
  const [membershipDetails, setMembershipDetails] = useState({
    freeExpiry: null,
    isFreeMember: false,
    isMembershipActive: false,
    isQuartelylyMember: false,
    isYearlyMember: false,
    quarterlyExpiry: null,
    yearlyExpiry: null,
    oneToOneExpiryDate: null,
    isOneToOneMember: false,
  });
  let subscriptionType = membershipDetails.isQuartelylyMember ? 1 : 2;

  useEffect(() => {
    setIsLoading(true);
    dispatch(accountActions.onGetUserInfo()).then((res) => {
      setIsLoading(false);
      if (res?.success) {
        setFirstName(res.data.firstName);
        setLastName(res.data.lastName);
        setEmail(res.data.email);
        setMembershipDetails(res.data.membership);
        setJobNotifications(res.data.jobNotification);
        setResourceNotifications(res.data.resourceNotification);
      } else {
        toast.error(res?.message);
        if (res?.message == 'Un paid member') {
          window.location.href = res.data.url + `/pricing/`;
        }
      }
    });
    if (moment(membershipDetails.freeExpiry, "YYYY/MM/DD").toDate() >= moment().toDate()) {
      setExpiryDateValid(true);
    }
    Amplitude('View Account Page');
  }, [subscriptionChange]);

  const validatePassword = (value) => {
    const testPassword = value.match(passwordRegx);
    if (testPassword !== null) {
      setIsValidPassword(true);
    } else {
      setIsValidPassword(false);
    }
  };

  const handleCancelSubs = (isCancel) => {
    setIsLoading(true);
    dispatch(accountActions.onCancelSubscription(subscriptionType, isCancel))
      .then((res) => {
        setIsLoading(false);
        setCancelSubsModalShow(false);
        toast.success(res.message);
        setSubscriptionChange(!subscriptionChange);
      })
      .catch(() => {
        setIsLoading(false);
        toast.error("Something went wrong.");
      });
  };

  const isEmpty = (value, label) => {
    if (isError && value === '') {
      return (
        <p className="errorTxt">Please enter {label}.</p>
      )
    }
  }

  const confirmPasswords = () => {
    if (newPassword !== newPasswordConfirm) {
      setMatchPasswords(false);
    }
    else {
      setMatchPasswords(true);
    }
  }

  const togglepasswordshow = () => {
    setCurrentPassword('');
    setNewPassword('');
    setNewPasswordConfirm('');
    setIsError(false);
    setChangePwd(!changePwd);
  }

  const toggleNotifications = () => {
    setNotifications(!notifications);
  }

  const onPressLogOut = () => {
    localStorage.removeItem('access_token');
    localStorage.removeItem('displayName');
    Amplitude("Logout Success");
    logout();
    navigate('/login');
  };

  const handleChangePassword = (e) => {
    e.preventDefault();
    confirmPasswords();
    if (currentPassword && newPassword && newPasswordConfirm  && isValidPassword && matchPasswords) {
      Amplitude("Password Update Failed");
      if (newPassword === currentPassword) {
        toast.error("The new password cannot be the same as the current password.");
      }
      else {
        setIsLoading(true);
        setIsError(false);
        dispatch(accountActions.onChangePassword(currentPassword, newPassword)).then((res) => {
          setIsLoading(false);
          if (res?.success) {
            Amplitude("Password Update Success");

            toast.success(res?.message);
            togglepasswordshow();
          }
          else {
            Amplitude("Password Update Failed");

            toast.error(res?.message);
          }
        });
      } 
    }
    else {
      setIsError(true);
    }
  }
  
  const isShowPassword = (val) => {
    if (val) {
      return ('text');
    }
    else {
      return ('password');
    }
  };

  const showPasswordIcon = (showPwd) => (
    showPwd ? <AiFillEyeInvisible /> : <AiFillEye />
  );

  const handleShowPassword = (showPassword, setShowPassword) => {
    setShowPassword(!showPassword);
  };
  
  const handleUpdateNotifications = (e) => {
    e.preventDefault();
      setIsLoading(true);
        setIsError(false);
        dispatch(accountActions.onUpdateNotifications(jobNotifications, resourceNotifications)).then((res) => {
          setIsLoading(false);
          if (res?.success) {
            Amplitude("Update Notification preference");
            toast.success(res?.message);
            toggleNotifications();
          }
          else {
            toast.error(res?.message);
          }
        });
  }
  return (
    <div>
      <ScreenContainer>
        {isLoading && <Loader />}
        <button className="mobileHeaderImg" onClick={()=>{
          Amplitude('Go to Homepage');
          navigate("/userdashboard")}}
        >
          <img src={GetBoardwiseHeader} alt="GetBoardwiseHeader" />
        </button>
        <div  className="accountPage">
        <Row className="dashboardSection">
            <Col className='col-sm-3 col-12'>
                <div className='dashboardHeadings'>
                    <h2>Basic Information</h2>
                    <p>Please <a href="mailto:john.hall@getboardwise.com">contact us</a> if you need to change this information.</p>
                </div>
            </Col>
            <Col className='col-sm-7 col-12'>
            <div className="siginFormWrpper">
                <div style={{display:"flex"}}>
                <TextField id="filled-basic" label="First Name" variant="filled" value={firstName} InputProps={{readOnly: true}}
            className="nametextbox" helperText="Read Only"/>
            <TextField id="filled-basic" label="Last Name" variant="filled" value={lastName} InputProps={{readOnly: true}}
            className="nametextbox" helperText="Read Only"/>
                </div>
            
            <TextField id="filled-basic" label="Email Address" variant="filled" value={email} InputProps={{readOnly: true}}
            className="nametextbox" helperText="Read Only"/>
                  <br />
                </div>
            </Col>     
        </Row>
        <hr />
        <Row className="dashboardSection">
            <Col className='col-sm-3 d-sm-block d-none'>
                <div className='dashboardHeadings'>
                    <h2>Password</h2>
                </div>
            </Col>
            <Col className='d-sm-none col-11'>
              <b>Modify your existing password.</b>
            </Col>
            <Col className='d-sm-none col-1 col-sm-2'>
                <div className="toggleButtonAccount">
                  <button onClick={togglepasswordshow}>{changePwd ? "Close" : "Edit"}</button>
                </div>
            </Col>      
            <Col className='col-sm-7 col-12'>
                <div className="changePasswordContainer">
                    {changePwd ? (
                        <Form onSubmit={handleChangePassword}>
                            <TextField 
                                label="Current Password" 
                                type={isShowPassword(showCurrentPwd)}
                                variant="filled"
                                className="nametextbox"
                                value={currentPassword}
                                onChange={(e) => {
                                    setCurrentPassword(e.target.value);
                                }}
                                InputProps={{
                                  endAdornment: (
                                    <InputAdornment position="end">
                                      <IconButton
                                        onClick={()=>handleShowPassword(showCurrentPwd, setShowCurrentPwd)}
                                      >
                                      {showPasswordIcon(showCurrentPwd)}
                                      </IconButton>
                                    </InputAdornment>
                                  )
                                }}
                            />
                            {isEmpty(currentPassword, "current password")}
                            <TextField 
                                label="New Password" 
                                type={isShowPassword(showNewPwd)}
                                variant="filled"
                                className="nametextbox"
                                value={newPassword}
                                onChange={(e) => {
                                    setNewPassword(e.target.value);
                                }}
                                onBlur={(e) => validatePassword(e.target.value)}
                                InputProps={{
                                  endAdornment: (
                                    <InputAdornment position="end">
                                      <IconButton
                                        onClick={()=>handleShowPassword(showNewPwd, setShowNewPwd)}
                                      >
                                        {showPasswordIcon(showNewPwd)}
                                      </IconButton>
                                    </InputAdornment>
                                  )
                                }}
                            />
                                <div className="pwdRules">Your password needs to:
                                  <ul>
                                    <li>include both upper and lower case letters</li>
                                    <li>include at least one number</li>
                                    <li>include at least one symbol</li>
                                    <li>be minimum 8 and maximum 16 characters long</li>
                                  </ul>
                                </div>
                            {isEmpty(newPassword, "new password")}
                            <TextField 
                                label="Confirm New Password" 
                                type={isShowPassword(showNewPwd2)}
                                variant="filled"
                                className="nametextbox"
                                value={newPasswordConfirm}
                                onChange={(e) => {
                                    setNewPasswordConfirm(e.target.value);
                                }}
                                onBlur={() => confirmPasswords()}
                                InputProps={{
                                  endAdornment: (
                                    <InputAdornment position="end">
                                      <IconButton
                                        onClick={()=>handleShowPassword(showNewPwd2, setShowNewPwd2)}
                                      >
                                        {showPasswordIcon(showNewPwd2)}
                                      </IconButton>
                                    </InputAdornment>
                                  )
                                }}
                            />
                            {isEmpty(newPasswordConfirm, "password")}
                            {comparePasswords(matchPasswords, newPasswordConfirm)}
                            <button className="saveButton">Save</button>
                        </Form>
                    )
                    :
                    (<b className='d-sm-block d-none'>Modify your existing password.</b>)}
                </div>
            </Col>
            <Col className='col-1 col-sm-2 d-sm-block d-none'>
                <div className="toggleButtonAccount">
                  <button onClick={togglepasswordshow}>{changePwd ? "Close" : "Edit"}</button>
                </div>
            </Col>
        </Row>
        <hr />
        <Row className="dashboardSection">
            <Col className='col-sm-3 d-sm-block d-none'>
                <div className='dashboardHeadings'>
                    <h2>Membership</h2>
                </div>
            </Col>
            <Col className='col-sm-7 col-11'>
                <div className="accountMembershipContainer">
                {(membershipDetails.isFreeMember && expiryDateValid) && <div>
                    <p className="membershipLevel">Complimentary Membership.</p>
                    <p>You have a complimentary trial valid till {membershipDetails.freeExpiry}.</p>  
                    </div>}
                  {(!membershipDetails.isFreeMember && !membershipDetails.isMembershipActive &&
                    !membershipDetails.isQuartelylyMember && !membershipDetails.isYearlyMember
                    && !membershipDetails.isOneToOneMember) && <div>
                    <p className="membershipLevel">Free Membership.</p>
                    <p className="membershipTextTop">A yearly membership is perfect for anyone who is looking to transition into their first or second Board appointment.</p> 
                    <ul>
                        <li>
                        Apply for our fully exclusive Board appointments in the private sector, from tech companies to extreme sports brands.
                        </li>
                        <li>
                        Make the most of our resources and quick reads covering topics from Board influence to Coaching and mentoring.
                        </li>
                        <li>
                        £115.00 + VAT / $138.00
                        </li>
                    </ul> 
                    </div>}
                    {(membershipDetails.isQuartelylyMember && toggleEditMembership) && <div>
                      <p className="membershipLevel">Quarterly Membership.</p>
                      {membershipDetails.isMembershipActive ? "Your membership auto-renews on " : "Your membership is valid till "}
                      {moment(membershipDetails.quarterlyExpiry).utc().format("DD/MM/YYYY")}.
                    </div>}
                    {(membershipDetails.isOneToOneMember) && <div>
                      <p className="membershipLevel">One To One Membership.</p>
                      Your membership is valid till
                      {moment(membershipDetails.oneToOneExpiryDate).utc().format("DD/MM/YYYY")}.
                    </div>}
                    {(membershipDetails.isYearlyMember && toggleEditMembership) && <div>
                      <p className="membershipLevel">Yearly Membership.</p>
                      {membershipDetails.isMembershipActive ? "Your membership auto-renews on " : "Your membership is valid till "}
                       {moment(membershipDetails.yearlyExpiry).utc().format("DD/MM/YYYY")}.
                    </div>}
                    {((membershipDetails.isQuartelylyMember || membershipDetails.isYearlyMember) && !toggleEditMembership) && <div>
                      <p className="membershipLevel">
                        Turn {membershipDetails.isMembershipActive ? "off" : "on"} auto renewal
                      </p>
                      <span>Please confirm that you wish to {membershipDetails.isMembershipActive && "not"} be billed again 
                      on {membershipDetails.yearlyExpiry && `${moment(membershipDetails.yearlyExpiry).utc().format("DD/MM/YYYY")} for another years membership. `}
                      {membershipDetails.quarterlyExpiry && `${moment(membershipDetails.quarterlyExpiry).utc().format("DD/MM/YYYY")} for another quarters membership. `} 
                        You can continue to make use of your premium features {membershipDetails.isMembershipActive && "until then"} including:</span>
                        <ul>
                          <li>Being part of one of the fastest growing networks of like minded professionals.</li>
                          <li>Access to our fully exclusive Board appointments in the private sector.</li>
                          <li>Exclusive resources.</li>
                        </ul>
                        {membershipDetails.isMembershipActive ? 
                        <div style={{display:"flex"}}>
                          <button className="saveButton jobModalButtonUpgrade" 
                          onClick={() => {
                            Amplitude('Upgrade Membership');
                            navigate('/pricing')}}
                          >
                            Upgrade
                          </button>
                          <button className="saveButton jobModalButtonLater" onClick={() =>setCancelSubsModalShow(true)}>
                            Cancel Membership
                          </button>
                        </div> : <button className="saveButton" onClick={()=>handleCancelSubs(false)}>Rejoin</button>}
                    </div>}
                </div>
            </Col>
            <Col className='col-1 col-sm-2'>
                <div className="toggleButtonAccount">
                  {(!membershipDetails.isFreeMember && !membershipDetails.isMembershipActive &&
                    !membershipDetails.isQuartelylyMember && !membershipDetails.isYearlyMember
                    && !membershipDetails.isOneToOneMember) && 
                    <button onClick={() => {
                      Amplitude('Upgrade Membership');
                      navigate('/pricing')}}>Upgrade</button>}
                    <button onClick={() => setToggleEditMembership(!toggleEditMembership)}>
                    {(membershipDetails.isQuartelylyMember || membershipDetails.isYearlyMember) &&
                     (toggleEditMembership ? "Edit" : "Close")}</button>
                </div>
            </Col>
        </Row>
        <hr />
        <Row className="dashboardSection">
            <Col className='col-sm-3 d-sm-block d-none'>
                <div className='dashboardHeadings'>
                    <h2>Notifications</h2>
                </div>  
            </Col>
            <Col className='col-11 col-sm-7'>
                <div className="changePasswordContainer">
                    {notifications ? (
                        <Form onSubmit={handleUpdateNotifications}>
                          <div className="notificationsLabel">New Jobs.</div>
                          <RadioGroup
                            value={jobNotifications}
                            onChange={(e)=>{setJobNotifications(e.target.value); setNotificationsDisabled(false)}}
                          >
                            <FormControlLabel value={true} 
                            control={<Radio sx={{color: '#0F2B18','&.Mui-checked': {color: '#FFCC00'}}}/>}
                            label="On" />
                            <FormControlLabel value={false} 
                            control={<Radio sx={{color: '#0F2B18','&.Mui-checked': {color: '#FFCC00'}}}/>}
                            label="Off" />
                          </RadioGroup>
                          <div className="notificationsLabel" style={{paddingTop: "20px"}}>
                            New resource centre articles.
                          </div>
                          <RadioGroup
                            value={resourceNotifications}
                            onChange={(e)=>{setResourceNotifications(e.target.value); setNotificationsDisabled(false)}}
                          >
                            <FormControlLabel value={true} 
                            control={<Radio sx={{color: '#0F2B18','&.Mui-checked': {color: '#FFCC00'}}}/>}
                            label="On" />
                            <FormControlLabel value={false} 
                            control={<Radio sx={{color: '#0F2B18','&.Mui-checked': {color: '#FFCC00'}}}/>}
                            label="Off" />
                          </RadioGroup>
                          <button className="saveButton" disabled={notificationsDisabled}>Save</button>
                        </Form>
                    )
                    :
                    (<div><b className='d-sm-block d-none'>Manage your notification settings.</b>
                    <b className='d-sm-none d-block'>Manage your notifications.</b></div>)}
                </div>
            </Col>
            <Col className='col-1 col-sm-2 d-sm-block'>
                <div className="toggleButtonAccount">
                  <button onClick={toggleNotifications}>{notifications ? "Close" : "Edit"}</button>
                </div>
            </Col>
        </Row>
        <hr className='d-md-none' />
        <Row className='d-md-none accountLogout'>
          <Col>
            <div>
              <button onClick={() => onPressLogOut()}>Logout</button>
            </div>
          </Col>
        </Row>
        </div>
      </ScreenContainer>
      <CancelSubsModal
          show={cancelSubsModalShow}
          onHide={() => setCancelSubsModalShow(false)}
          handleCancelSubs={()=>handleCancelSubs(true)}
        />
    </div>
  );
}
export default UserProfile;
