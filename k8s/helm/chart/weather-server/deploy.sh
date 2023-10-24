chart_version="0.1.0"

helm package .
helm -n weather delete weather-server
helm -n weather install weather-server ./weather-server-$chart_version.tgz

kubectl wait --namespace weather --for=condition=ready pod --selector=app.kubernetes.io/name=weather-server --timeout=15s
kubectl -n weather get pods
