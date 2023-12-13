#! /bin/sh

until pids=$(pidof $1)
do
    echo 'waiting'
    sleep 1
done
echo 0
