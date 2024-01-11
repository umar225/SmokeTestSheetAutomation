import React, { useState, useEffect} from 'react';
import { accountActions } from '../store/account/accountActions';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import Loader from '../components/Loader';
import { Boardwise_logo_new } from '../utils/image';
import { useAuth } from '../routes/useAuth';

const LinkedInRedirect = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const { login } = useAuth();

    const [message, setMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [isSuccess, setIsSuccess] = useState(false);

    const code = new URLSearchParams(window.location.search).get(
        'code'
      );

    useEffect(()=>{
        setIsLoading(true);
        dispatch(accountActions.onLinkedInUserLogin('authorization_code', code, window.location.origin+"/linkedin-redirect")).then((res) => {
            setIsLoading(false);
            setMessage(res.message);
            if (res?.success) {
                setIsLoading(true);
                dispatch(accountActions.onExternalLogin('linkedin', res.data.access_token)).then((res)=> {
                    setIsLoading(false);
                    setMessage(res.message);
                    if (res.success) {
                        setIsSuccess(true);
                        if (res.data?.role === 'customer') {
                            login('customer');
                          } 
                    }
                })
            }
    })
},[])
  
  return (
    <div className="mainWrapper">
    {isLoading && <Loader />}
      <div className="grayBackground">
        <div className="loginWrapper">
          <div className="loginHeader">
            <img className="bLogo" src={Boardwise_logo_new} alt="" />
          </div>
          {message !== '' && <div className={`login-modal ${isSuccess ? 'message-success' : 'message-error'}`}>{message}</div>}
          <div className="signInWrapper">
            <div className="siginFormWrpper">
            <button className="loginBtn"  onClick={() => navigate('/login')}>
                <span className="loginTxt">Back to Sign In</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LinkedInRedirect;