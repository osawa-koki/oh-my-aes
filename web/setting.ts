
const Env = require('./next.config.js');
const isProd = process.env.NODE_ENV === 'production';

const setting = {
  isProd,
  basePath: Env.basePath,
  apiPath: isProd ? '' : 'http://localhost:8000',
  title: '🐸 Next.js Template 🐸',
  smallWaitingTime: 100,
  waitingTime: 1000,
};

export default setting;
