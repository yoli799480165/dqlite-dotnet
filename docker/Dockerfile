FROM ubuntu as dqlite-lib-builder 
ARG DEBIAN_FRONTEND="noninteractive"
ENV TZ=Europe/London
ENV LD_LIBRARY_PATH=/usr/local/lib
ENV GOROOT=/usr/local/go
ENV GOPATH=/go
ENV PATH=$GOPATH/bin:$GOROOT/bin:$PATH

RUN apt-get update && apt-get install -y git build-essential dh-autoreconf pkg-config libuv1-dev libsqlite3-dev liblz4-dev tcl8.6 wget

WORKDIR /opt/raft
COPY native/raft .
RUN autoreconf -i && ./configure && make && make install

WORKDIR /opt/dqlite
COPY native/dqlite .
RUN autoreconf -i && ./configure && make && make install

FROM mcr.microsoft.com/dotnet/sdk:7.0 as dotnet-builder
ARG DEBIAN_FRONTEND="noninteractive"
ENV TZ=Europe/London
ENV LD_LIBRARY_PATH=$PATH:/usr/local/lib

COPY --from=dqlite-lib-builder /usr/local/lib /usr/local/lib
COPY --from=dqlite-lib-builder \
    /usr/lib/x86_64-linux-gnu/libuv.so \
    /usr/lib/x86_64-linux-gnu/libuv.so.1\
    /usr/lib/x86_64-linux-gnu/libuv.so.1.0.0\
    /usr/lib/x86_64-linux-gnu/
COPY --from=dqlite-lib-builder \
    /lib/x86_64-linux-gnu/libsqlite3.so \
    /lib/x86_64-linux-gnu/libsqlite3.so.0 \
    /usr/lib/x86_64-linux-gnu/  

WORKDIR /opt/dqlite-dotnet
