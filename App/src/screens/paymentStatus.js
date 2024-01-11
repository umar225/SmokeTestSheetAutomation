import React from "react";
import { loadStripe } from "@stripe/stripe-js";
import { Elements } from "@stripe/react-stripe-js";
import myEnv from "../utils/env.json";
import PayStatus from "../components/PayStatus";
import { Header } from '../components/header/index';
import Footer from '../components/footer/footer';
import "../styles/stripeCheckout.css"

const stripePromise = loadStripe(myEnv.StripePublishKey);

export default function PaymentStatus() {
  
  return (
    <Elements stripe={stripePromise}>
            <Header />
            <PayStatus />
            <div className="footerWrapper">
              <Footer />
            </div>
          </Elements>
  );
}