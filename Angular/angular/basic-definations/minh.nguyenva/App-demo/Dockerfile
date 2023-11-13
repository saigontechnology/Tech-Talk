################
# Run in NGINX #
################

FROM nginx:1.23.0-alpine

ARG DIST_FOLDER_PATH=-sqa

COPY dist/${DIST_FOLDER_PATH} /opt/website
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
