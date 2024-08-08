# Use an Alpine base image
FROM alpine:3.14

# Set the Vault version
ENV VAULT_VERSION=1.17.2

# Create a directory for Vault
RUN mkdir /vault

# Install dependencies
RUN apk --no-cache add \
      bash \
      ca-certificates \
      wget \
      curl

# Download and install Vault
RUN wget --quiet --output-document=/tmp/vault.zip https://releases.hashicorp.com/vault/${VAULT_VERSION}/vault_${VAULT_VERSION}_linux_amd64.zip && \
    unzip /tmp/vault.zip -d /vault && \
    rm -f /tmp/vault.zip && \
    chmod +x /vault

# Update PATH to include Vault binary location
ENV PATH="PATH=$PATH:$PWD/vault"

# Copy Vault configuration file
COPY ./config/config.json /vault/config/config.json

# Print debug information to verify the file is copied
RUN ls -l /vault/config

# Expose Vault port
EXPOSE 8200

# Set the default command to run Vault
ENTRYPOINT ["vault"]