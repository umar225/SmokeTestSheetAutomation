import React from 'react';
import Card from 'react-bootstrap/Card';
import { useNavigate } from 'react-router-dom';
import Tooltip from '@mui/material/Tooltip';
import PropTypes from "prop-types"

export default function ResourceCard(props) {
  const navigate = useNavigate();
  const resource = props.resource;
  const title = resource.title;

  return (
    <Card
      className="jobCard"
      onClick={() => {
        navigate(`/resourcecenter/` + resource.url, {
          state: {
            url: resource.url,
          },
        });
      }}
    >
      <Card.Body className="resourceCardBody">
        <div>
          <Card.Img
            variant="left"
            src={resource.media[0].url}
          />
          <Card.Text className="resourceCardData">
          <div className='resourceCardHeader'>
            <Tooltip title={title}>
              <p className="jobTitleText jobCardTextColor">{title}</p>
            </Tooltip>
            <p className='previewTextCard resourceText'>{resource.preview}</p>
          </div>
        </Card.Text>
        </div>
      </Card.Body>
    </Card>
  );
}

ResourceCard.propTypes = {
  resource: PropTypes.shape({
    title: PropTypes.string,
    url: PropTypes.string,
    media: PropTypes.arrayOf(PropTypes.shape),
    preview: PropTypes.string,
    time: PropTypes.string,
  })
}