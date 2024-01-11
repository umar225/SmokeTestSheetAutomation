import React, { useState, useEffect } from 'react';
import { loadStripe } from '@stripe/stripe-js';
import { Elements } from '@stripe/react-stripe-js';
import CheckoutForm from '../components/CheckoutForm';
import { checkoutActions } from '../store/checkout/checkoutActions';
import Loader from '../components/Loader';
import { useDispatch } from 'react-redux';
import '../styles/stripeCheckout.css';
import myEnv from '../utils/env.json';
import { Header } from '../components/header/index';
import Footer from '../components/footer/footer';

// Make sure to call loadStripe outside of a component’s render to avoid
// recreating the Stripe object on every render.
// This is your test publishable API key.
const stripePromise = loadStripe(myEnv.StripePublishKey);


export default function Checkout() {
  const [clientSecret, setClientSecret] = useState('');
  const [intentId, setIntentId] = useState('');
  const [message, setMessage] = useState(null);
  const [isLoading, setIsLoading] = React.useState(false);
  const dispatch = useDispatch();
  const cartItems = JSON.parse(localStorage.getItem('finalBasket'));

  useEffect(() => {
    window.scrollTo(0, 0);
    // Get PaymentIntent as soon as the page loads
    setIsLoading(true);
    const orderData = JSON.parse(localStorage.getItem('orderData'));
    let intentId;
    if (orderData?.intent) {
      intentId = orderData.intent;
      dispatch(checkoutActions.getIntentId({ intentId: intentId })).then(
        (data) => {
          setIsLoading(false);
          if (data?.success) {
            setClientSecret(data.data.clientSecret);
            setIntentId(data.data.intentId);
          } else if (data.message) {
              setMessage(data.message);
            } else {
              setMessage(
                'Issue in loading the page, payment information is not available right now.'
              );
            }
        }
      );
    } else {
      setIsLoading(false);
      setMessage(
        'Issue in loading the page, payment information is not available right now.'
      );
    }
  }, []);

  const appearance = {
    theme: 'stripe',
    variables: {
      colorPrimary: '#00352B',
      colorBackground: '#ffffff',
      colorText: '#00353G',
      colorDanger: '#df1b41',
      fontFamily: 'Open Sans, sans-serif',
      spacingUnit: '2px',
      borderRadius: '4px',
      // See all possible variables below
    },
  };

  const options = {
    clientSecret,
    appearance,
  };

  return (
    <div>
      <Header />
      <div className="bodyWrapper">
        <div className="stripeF whiteBackground">
          {isLoading && <Loader />}
          {message && (
            <div id="payment-message" className="text-center">
              {message}
            </div>
          )}
          {clientSecret && (
            <Elements options={options} stripe={stripePromise}>
              <CheckoutForm intentId={intentId} cartItems={cartItems} />
            </Elements>
          )}
        </div>
      </div>
      <div className="footerWrapper">
        <Footer />
      </div>
    </div>
  );
}
