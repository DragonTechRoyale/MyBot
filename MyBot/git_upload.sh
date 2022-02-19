#!/bin/bash

echo "Commit message:"
read commit_message
if test "$commit_message" != ""; then
    git add -A 
    git commit -m "$commit_message"
    git push
fi
