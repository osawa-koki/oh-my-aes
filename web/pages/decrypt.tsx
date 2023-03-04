import { useState } from "react";
import { Alert, Button, Form, Spinner } from "react-bootstrap";
import Layout from "../components/Layout";
import setting from "../setting";
import { MyResponseType } from "../src/ResponseType";

export default function EncryptPage() {

  const [content, setContent] = useState<string>('UHitH3n3gDg5cn1SCsSFanIuZSRXK2hX7G+opdRbivM=');
  const [key, setKey] = useState<string>('my-key');
  const [decrypted, setDecrypted] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [copied, setCopied] = useState<boolean>(false);
  const [waiting, setWaiting] = useState<boolean>(false);

  const Encrypt = async () => {
    try {
      setWaiting(true);
      await new Promise(resolve => setTimeout(resolve, setting.smallWaitingTime));
      await fetch(`${setting.apiPath}/api/cipher/aes/ecb/decrypt/256?key=${key}&data=${encodeURIComponent(content)}`)
        .then(async (res) => {
          if (res.status === 500) {
            throw new Error(`サーバサイドで予期せぬエラーが発生しました。`);
          }
          if (res.status === 400) {
            throw new Error(`復号できないデータです。`);
          }
          if (res.status !== 200) {
            throw new Error(`${res.statusText}`);
          }
          return res.json();
        })
        .then((json: MyResponseType) => {
          setError(null);
          setDecrypted(json.decrypted);
        })
        .catch(e => {
          setError(`${e.message}`);
        }).finally(() => {
          setWaiting(false);
        });
    } catch (e) {
      setError(`${e}`);
    }
  };

  return (
    <Layout>
      <div id="Encrypt" className="mt-3">
        <h1>復号ツール</h1>
        <Form.Group className="mt-3">
          <Form.Label>暗号化された文字列</Form.Label>
          <Form.Control as="textarea" rows={5} value={content} onInput={(e) => {setContent((e.target as HTMLTextAreaElement).value)}} />
        </Form.Group>
        <Form.Group className="mt-3">
          <Form.Label>暗号化キー</Form.Label>
          <Form.Control type="text" placeholder="Enter Key" value={key} onInput={(e) => {setKey((e.target as HTMLTextAreaElement).value)}} />
        </Form.Group>
        {
          waiting ? (
            <div className="mt-3 d-flex justify-content-between">
              <Spinner animation="grow" variant="primary" />
              <Spinner animation="grow" variant="secondary" />
              <Spinner animation="grow" variant="success" />
              <Spinner animation="grow" variant="danger" />
              <Spinner animation="grow" variant="warning" />
              <Spinner animation="grow" variant="info" />
              <Spinner animation="grow" variant="light" />
              <Spinner animation="grow" variant="dark" />
            </div>
          ) : (
            <Button variant="primary" onClick={Encrypt} className="mt-3 d-block m-auto">復号 🔏</Button>
          )
        }
        <hr />
        {
          error !== null ? (
            <>
              <Alert variant="danger" className="mt-3">
                <Alert.Heading>復号に失敗しました。</Alert.Heading>
                {error}
              </Alert>
            </>
          ) : (
            <>
            {
              decrypted !== null && (
                <>
                  <Alert variant="info" className="mt-3">
                  復号された文字列は以下の通りです。
                  </Alert>
                  <Form.Group className="mt-3">
                    <Form.Label>復号された文字列</Form.Label>
                    <Form.Control readOnly as="textarea" value={decrypted} rows={5} />
                  </Form.Group>
                  <Button variant="warning" className="mt-3 d-block m-auto" onClick={async () => {
                    setCopied(true);
                    navigator.clipboard.writeText(decrypted);
                    await new Promise(resolve => setTimeout(resolve, setting.waitingTime));
                    setCopied(false);
                  }} disabled={copied}>
                    {copied ? 'コピーしました' : 'コピー'}
                  </Button>
                </>
              )
            }
            </>
          )
        }
      </div>
    </Layout>
  );
};
