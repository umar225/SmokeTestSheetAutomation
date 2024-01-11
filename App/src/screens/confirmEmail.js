import React, { useState, useEffect } from 'react';
import { Boardwise_logo_new } from '../utils/image';
import Loader from '../components/Loader';
import { accountActions } from '../store/account/accountActions';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

const ConfirmEmail = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [message, setMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);

  const urlString = window.location.search;
  const urlParams = new URLSearchParams(urlString);
  const userId = urlParams.get('userId');
  let token = urlParams.get('token');
  token = token.replace(/ /g, '+');

  useEffect(() => {
    setIsLoading(true);
    dispatch(accountActions.onConfirmEmail(userId, token)).then((res) => {
      setIsLoading(false);
        if (res?.success) {
          setIsSuccess(true);
          setMessage(res.message);
        }
        else {
          setMessage("There was a problem while verifying your email address.")
        }
    });
  }, []);

  return (
    <div className="mainWrapper">
      {isLoading && <Loader />}
        <div className="grayBackground">
          <div className="loginWrapper">
            <div className="loginHeader">
              <img className="bLogo" src={Boardwise_logo_new} alt="" />
            </div>
            {message !== '' && <span className={`login-modal confirm-email ${isSuccess ? 'message-success' : 'message-error'}`}>{message}
            {!isSuccess && 
            <span>{' '}Please{' '} 
            <button className="resendEmail" onClick={() => navigate('/resendConfirmation')}>
            click here </button> to resend verification email.</span>}
            </span>}
            <div className="signInWrapper">
              <div className="siginFormWrpper">
              <button className="loginBtn"  onClick={() => navigate('/login')}>
                  <span className="loginTxt">Go to Sign In</span>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
  );
};

export default ConfirmEmail;
