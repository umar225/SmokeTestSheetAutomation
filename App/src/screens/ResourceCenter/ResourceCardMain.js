import React from 'react';
import Amplitude from '../../utils/Amplitude';
import StarIcon from '@mui/icons-material/Star';
import { useNavigate } from 'react-router-dom';
import PropTypes from "prop-types"


export default function ResourceCardMain(props) {
  const resource = props.resource;
  const navigate = useNavigate();

  return (
    <div className="resourceBoxDiv">
      <button
        key={resource.id}
        className="resourceBox"
        onClick={() => {
          navigate(`/resourcecenter/${resource.url}`, {
            state: {
              url: resource.url,
            },
          });
        }}
      >
        <div className="boxHeader">
        {resource.isFeatured && (
            <div className="jobCardLocation appliedJob">
            <div className="jobSmallImageContainer">
              <StarIcon
                sx={{ fontSize: 12, color: "#0F2B18" }}
                className="rounded mx-auto d-block"
              />
              <span>
                Featured
              </span>
            </div>
          </div>
          )}
          <span className="imageContainer">
            <img src={resource.media[0].url} alt="resource" />
          </span>
          <span className="resourceTitle">
            {resource.title}
          </span>
          <span className="resourcePreview">
            {resource.preview}
          </span>
        </div>
      </button>
    </div>
  );
}


ResourceCardMain.propTypes = {
  resource: PropTypes.shape({
    id: PropTypes.string,
    title: PropTypes.string,
    url: PropTypes.string,
    media: PropTypes.arrayOf(PropTypes.shape),
    preview: PropTypes.string,
    time: PropTypes.string,
    isFeatured: PropTypes.bool
  })
}

