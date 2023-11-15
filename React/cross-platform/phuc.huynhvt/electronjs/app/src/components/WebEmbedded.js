import React from "react";

const WebEmbedded = () => {
  const url = "https://www.youtube.com/embed/-Lx8bV0rKQA?si=6oD_hnhg1hsAqo5X";

  return (
    <>
      <h3>4/ Web Embedded</h3>
      <div className="webView" id="webView">
        <iframe
          width="560"
          height="315"
          src={url}
          frameBorder="0"
          allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
          allowFullScreen
          title="Embedded YouTube Video"
        ></iframe>
      </div>
    </>
  );
};

export default WebEmbedded;
