import React, { useState } from 'react';
import {
  useStripe,
  useElements,
  CardElement
} from '@stripe/react-stripe-js';
import { useDispatch } from 'react-redux';
import Loader from '../../components/Loader';
import { checkoutActions } from '../../store/checkout/checkoutActions';
import { toast } from 'react-toastify';
import  PropTypes from 'prop-types';


export default function UserForm(props) {
  const stripe = useStripe();
  const elements = useElements();
  const [isLoading, setIsLoading] = useState(false);
  const [message, setMessage] = useState(null);
  const dispatch = useDispatch();
  const displayName = localStorage.getItem('displayName');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    if (!stripe || !elements) {
      return;
    }
    fetch(`${window.env?.URL}payment/customer`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        name: displayName,
        email: props.email,
        intentId: props.intentId,
        address: {
          country: "UK",
        },
      }),
    })
      const result = await stripe.createPaymentMethod({
        type: 'card',
        card: elements.getElement(CardElement),
      })

      if (result.error) {
        setMessage(result.error.message);
      } else {
        dispatch(checkoutActions.createSubscription({email: props.email, priceId: props.priceId, paymentMethodId: result.paymentMethod.id})).then((res) => {
          if (res.success) {
            toast.success("Subscribed Successfully");
            props.setIsMember(true);
          }
          else {
            toast.error(res.message);
          }
            setIsLoading(false);
        });
      }
    setIsLoading(false);
  };

  const options = {
    fields: {
      billingDetails: 'never',
    },
    style: {
        base: {
          fontSize: '16px',
          color: '#32325d',
          fontFamily: 'Arial, sans-serif',
          '::placeholder': {
            color: '#aab7c4',
          },
        },
      },
  };

  return (
    <form id="payment-form" className="row" onSubmit={handleSubmit}>
        {isLoading && <Loader />}
          <div className="stripeBackground">
            <CardElement id="card-element" options={options} />
            <button
                  disabled={ !stripe || !elements}
                  id="submit"
                  className="saveButton"
                >
                  <span id="button-text">
                      Submit
                  </span>
                </button>
                {message && <div id="payment-message">{message}</div>}
          </div>
    </form>
  );
}

UserForm.propTypes = {
  setIsMember: PropTypes.func,
  email: PropTypes.string,
  intentId: PropTypes.string,
  priceId: PropTypes.string,

}

