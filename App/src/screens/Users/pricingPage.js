import React, { useEffect, useState } from 'react';
import { ScreenContainer } from '../../components/ScreenContainer';
import { useDispatch } from 'react-redux';
import { accountActions } from '../../store/account/accountActions';
import Loader from '../../components/Loader';
import Accordion from '@mui/material/Accordion';
import AccordionDetails from '@mui/material/AccordionDetails';
import AccordionSummary from '@mui/material/AccordionSummary';
import Polygon4 from "../../utils/images/Polygon4.png";
import GetBoardwiseHeader from '../../utils/images/getBoardwiseHeader.svg';
import { useNavigate } from 'react-router-dom';
import Amplitude from '../../utils/Amplitude';

export default function PricingPage() {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [freeMembership, setFreeMembership] = useState(false);
  const [quarterlyMembership, setQuarterlyMembership] = useState(false);
  const [yearlyMembership, setYearlyMembership] = useState(false);
  const [one2oneMembership, setOne2oneMembership] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [expanded, setExpanded] = React.useState(false);
  const [width, setWidth] = useState(window.innerWidth);
  const breakpoint = 600;
  let currentMembership = "free member";

  const data = [
    {
      heading: "Free",
      pricingInfo: "An introduction into the world of Boardwise.",
      subscriptionPrice: "£0",
      vat: "",
      listPoints: [{ __html: "Access to <b>3 articles</b> from our resource centre."}, 
      { __html: "Browse our <b>jobs board</b> with censored exclusive and external roles."}],
      disableB: freeMembership,
      border: freeMembership,
      buttonText: "Current Plan",
      handleBClick: null,
      bestValue: false,
    },
    {
      heading: "Quarterly",
      pricingInfo: "Fresh insights, networking, and resources every 3 months!",
      subscriptionPrice: "£115",
      vat: "+VAT",
      listPoints: [{ __html: "All NED, Advisory and Consultancy <b>roles in one place.</b>"}, 
      { __html: "Full access to <b>resource & learning centre.</b>"}],
      disableB: quarterlyMembership || yearlyMembership,
      border: quarterlyMembership,
      buttonText: `${quarterlyMembership ? "Current Plan" : "Subscribe"}`,
      handleBClick: ()=>handleClick(1),
      bestValue: false,
    },
    {
      heading: "Yearly",
      pricingInfo: "Fuel your success with an annual plan and a bespoke CV rewrite.",
      subscriptionPrice: "£250",
      vat: "+VAT",
      listPoints: [{ __html: "All benefits of the <b>Quarterly</b> subscription."}, 
      { __html: "A professionally written bespoke <b>CV rewrite.</b>"},
      { __html: "Extended Access for <b>12 months.</b>"}],
      disableB: yearlyMembership,
      border: false,
      buttonText: `${yearlyMembership ? "Current Plan" : "Subscribe"}`,
      handleBClick: ()=>handleClick(2),
      bestValue: true,
    },
    {
      heading: "1-1",
      pricingInfo: "Proactive guidance and hands on support to secure your next opportunity.",
      subscriptionPrice: "£POA",
      vat: "",
      listPoints: [{ __html: "12 week <b>hyper-targeted messaging campaign.</b>"}, 
      { __html: "<b>12 months</b> of full access to the Boardwise platform."},
      { __html: "<b>1-1 support</b> via Whatsapp Business."},
      { __html: "Increase your <b>LinkedIn profile activity</b> including; followers and connections."},
      { __html: "<b>10 changes</b> to the messaging and target audience."},
      { __html: "LinkedIn profile <b>optimisation.</b>"},
      { __html: "<b>Agreement</b> and <b>contract</b> templates."},
      { __html: "1 page <b>CV rewrite.</b>"}
    ],
      disableB: one2oneMembership,
      border: false,
      buttonText: `${one2oneMembership ? "Current Plan" : "Book a call"}`,
      handleBClick: ()=>bookCall(),
      bestValue: false,
      }
]

  const reversedData = data.toReversed();

  useEffect(() => {
    const handleWindowResize = () => setWidth(window.innerWidth);
    window.addEventListener('resize', handleWindowResize);
    setIsLoading(true);
    dispatch(accountActions.getPricingDetails()).then((res) => {
      setIsLoading(false);
      setFreeMembership(!res.data.isQuartelylyMember && !res.data.isYearlyMember && !res.data.isOneToOneMember);
      setQuarterlyMembership(res.data.isQuartelylyMember && !res.data.isYearlyMember && !res.data.freeMembership);
      setYearlyMembership(res.data.isYearlyMember && !res.data.isQuartelylyMember && !res.data.freeMembership);
      setOne2oneMembership(res.data.isOneToOneMember && !res.data.isQuartelylyMember && !res.data.isYearlyMember);
      if (res.data.isQuartelylyMember) currentMembership="quarterly subscription";
      if (res.data.isYearlyMember) currentMembership="yearly subscription";
      if (res.data.isOneToOneMember) currentMembership="one to one member";
      Amplitude('View Pricing page', {
        'Membership_Plan': currentMembership,
      })
    });
    },[])

  const handleClick = (type) => {
    setIsLoading(true);
    if (type === 1) {
      Amplitude('Checkout with Quarterly Plan', {
        'Membership_Plan': currentMembership,
      })
    }
    if (type === 2) {
      Amplitude('Checkout with Yearly Plan', {
        'Membership_Plan': currentMembership,
      })
    }
    dispatch(accountActions.onCreateCheckout(type)).then((res) => {
      setIsLoading(false);
      if (res.success) {
        window.location.href = `${res.message}`;
      }
      })
  }

  const handleChange = (panel) => (event, isExpanded) => {
    setExpanded(isExpanded ? panel : false);
  };

  const bookCall = () => {
    Amplitude('Book 1-2-1 Plan', {
      'Membership_Plan': currentMembership,
    })
    window.open("https://meetings-eu1.hubspot.com/austin-b", "_blank");
    navigate('/userdashboard');
  }

    return (
      <div>
        <ScreenContainer className="noPaddingMargin">
          {isLoading && <Loader />}
          <button className="mobileHeaderImg" onClick={()=>{
            Amplitude('Go to Homepage');
            navigate("/userdashboard")}}
          >
            <img src={GetBoardwiseHeader} alt="GetBoardwiseHeader" />
          </button>
          <div className="noPaddingMargin">
            <h1 className='pricingPageTitle'>Ready to unlock your boardroom potential?</h1>
            {width > breakpoint ? (
              <div className="flexBox">
                <div className="pricingPageDiv">
                {data.map((data)=>(
                  <div key={data.heading}>
                    {data.bestValue && <p className='bestValueText'>BEST VALUE</p>}
                  <div className={`pricingCard ${data.bestValue && "bestValue"} ${data.border && "cardBorder"}`}>
                    <h3>{data.heading}</h3>
                    <hr className="pricingSeparator" />
                    <div className="pricingInfo">
                      {data.pricingInfo}
                    </div>
                    <div className="subscriptionPrice">
                      {data.subscriptionPrice} <span>{data.vat}</span>
                    </div>
                    <div style={{display: `${data.heading==="Free" && !freeMembership ? "none" : "block"}`}}>
                      <button className="saveButton" disabled={data.disableB} onClick={data.handleBClick}>
                        {data.buttonText}
                      </button>
                    </div>
                    <ul>
                      {data?.listPoints.map((item)=><li dangerouslySetInnerHTML={item} key={item} />)}
                    </ul>
                  </div>
                </div>
                ))}
              </div>
            </div>
            ) : (
            reversedData.map((data)=>(
            <Accordion expanded={expanded === data.heading} onChange={handleChange(data.heading)} 
              className={`pricingPageDivMob ${data.bestValue && "bestValueMob"}`}
              key={data.heading}
            >
              <AccordionSummary
                expandIcon={<img src={Polygon4} alt="expand" />}
              >
                <div className="pricingMobAcc">
                  <div className="pricingMobTitle">
                    <h3>{data.heading}</h3>
                    <div className="subscriptionPrice">
                      {data.subscriptionPrice} <span>{data.vat}</span>
                    </div>
                  </div>
                  <div className="pricingInfo">
                    {data.pricingInfo}
                  </div>
                </div>
              </AccordionSummary>
              <AccordionDetails>
                <ul>
                  {data?.listPoints.map((item)=><li dangerouslySetInnerHTML={item} key={item} />)}
                </ul>
                <div style={{display: `${data.heading==="Free" && !freeMembership ? "none" : "block"}`}}>
                  <button className="saveButton" disabled={data.disableB} onClick={data.handleBClick}>
                    {data.buttonText}
                  </button>
                </div>
              </AccordionDetails>
            </Accordion>))
            )}
          </div>
        </ScreenContainer>
      </div>
    );
}
