#!/bin/bash

NUGET_PATH=~/.nuget/packages
TOOLS_PATH=$NUGET_PATH/Grpc.Tools/1.19.0/tools/macosx_x64
INCLUDE_PATH=$NUGET_PATH/grpc.tools/1.19.0/build/native/include/google/protobuf

%TOOLS_PATH%/protoc -I$PWD/../../protos;$INCLUDE_PATH --csharp_out $PWD/grpc_generated $PWD../../protos/properties.proto --grpc_out $PWD/grpc_generated --plugin=protoc-gen-grpc=$TOOLS_PATH/grpc_csharp_plugin
